using ParrelSync;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;
using UnityEngine.UI;

public class SimpleMatchmaking : MonoBehaviour
{
    [SerializeField] private Button _classicBtn;

    private UnityTransport _transport;
    private Lobby _connectedLobby;
    private const string JoinCodeKey = "j";
    private string _playerId;

    private void Awake()
    {
        _transport = FindObjectOfType<UnityTransport>();
    }

    public async Task CreateOrJoinLobby()
    {
        await Autheticate();

        _connectedLobby = await QuickJoinLobby() ?? await CreateLobby();
    }

    private async Task Autheticate()
    {
        var options = new InitializationOptions();

#if UNITY_EDITOR
        // Parellsync
        options.SetProfile(ClonesManager.IsClone() ? ClonesManager.GetArgument() : "Primary");
#endif
        await UnityServices.InitializeAsync(options);

        await AuthenticationService.Instance.SignInAnonymouslyAsync();
        var userId = AuthenticationService.Instance.PlayerId;
    }

    private async Task<Lobby> QuickJoinLobby()
    {
        try
        {
            // Attempt to join a lobby in progress
            var lobby = await Lobbies.Instance.QuickJoinLobbyAsync();

            // If we found one, grab relay allocation details
            var a = await RelayService.Instance.JoinAllocationAsync(lobby.Data[JoinCodeKey].Value);

            // Set the details to the tranform
            SetTransformAsClient(a);

            // Join game room as client
            NetworkManager.Singleton.StartClient();
            return lobby;
        }
        catch (Exception e)
        {
            Debug.Log("No lobbies available via quick join");
            return null;
        }
    }

    private async Task<Lobby> CreateLobby()
    {
        try
        {
            const int maxPlayers = 10;

            // Create allocation and join code to share with the lobby
            var a = await RelayService.Instance.CreateAllocationAsync(maxPlayers);
            var joinCode = await RelayService.Instance.GetJoinCodeAsync(a.AllocationId);

            // Create lobby and add the relay join code to the lobby data
            var options = new CreateLobbyOptions
            {
                Data = new Dictionary<string, DataObject> { { JoinCodeKey, new DataObject(DataObject.VisibilityOptions.Public, joinCode) } }
            };

            var lobby = await Lobbies.Instance.CreateLobbyAsync("New lobby", maxPlayers, options);

            // Kepp lobby alive
            StartCoroutine(HeartbeatLobbyCoroutine(lobby.Id, 15));

            // Set game room to use the relay allocation
            _transport.SetHostRelayData(a.RelayServer.IpV4, (ushort)a.RelayServer.Port, a.AllocationIdBytes, a.Key, a.ConnectionData);

            NetworkManager.Singleton.StartHost();
            return lobby;
        }
        catch (Exception e)
        {
            Debug.Log("Failed to create a lobby");
            return null;
        }
    }

    private void SetTransformAsClient(JoinAllocation a)
    {
        _transport.SetClientRelayData(a.RelayServer.IpV4, (ushort)a.RelayServer.Port, a.AllocationIdBytes, a.Key, a.ConnectionData, a.HostConnectionData);
    }

    private static IEnumerator HeartbeatLobbyCoroutine(string lobbyId, float waitTimeSeconds)
    {
        var delay = new WaitForSecondsRealtime(waitTimeSeconds);
        while(true)
        {
            Lobbies.Instance.SendHeartbeatPingAsync(lobbyId);
            yield return delay;
        }
    }

    private void OnDestroy()
    {
        try
        {
            StopAllCoroutines();

            if(_connectedLobby != null)
            {
                if(_connectedLobby.HostId == _playerId)
                {
                    Lobbies.Instance.DeleteLobbyAsync(_connectedLobby.Id);
                }
                else
                {
                    Lobbies.Instance.RemovePlayerAsync(_connectedLobby.Id, _playerId);
                }
            }
        }
        catch (Exception e)
        {
            Debug.Log($"Error shutting down lobb: {e}");
        }
    }
}

