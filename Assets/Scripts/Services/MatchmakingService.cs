using Assets.Scripts.Services;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using Unity.Services.Relay;
using UnityEngine;

public static class MatchmakingService
{
    private const int HeartbeatInterval = 15;
    private const int LobbyRefreshRate = 2;

    private static UnityTransport _transport;

    private static Lobby _connectedLobby;
    private static CancellationTokenSource _heartbeatSource, _updateLobbySource;

    private static string JoinCodeKey = "j";

    private static UnityTransport Transport
    {
        get => _transport != null ? _transport : _transport = UnityEngine.Object.FindObjectOfType<UnityTransport>();
        set => _transport = value;
    }

    public static event Action<Lobby> CurrentLobbyRefreshed;

    public async static Task CreateOrJoinLobby()
    {
        await MyAuthenticationService.Login();

        _connectedLobby = await QuickJoinLobby() ?? await CreateLobby();

        Debug.Log(_connectedLobby.Id);
    }

    public static void ResetStatics()
    {
        if (Transport != null)
        {
            Transport.Shutdown();
            Transport = null;
        }

        _connectedLobby = null;
    }

    private async static Task<Lobby> QuickJoinLobby()
    {
        try
        {
            // Attempt to join a lobby in progress
            var lobby = await Lobbies.Instance.QuickJoinLobbyAsync();

            // If we found one, grab relay allocation details
            var a = await RelayService.Instance.JoinAllocationAsync(lobby.Data[JoinCodeKey].Value);

            // Set the details to the tranform
            Transport.SetClientRelayData(a.RelayServer.IpV4, (ushort)a.RelayServer.Port, a.AllocationIdBytes, a.Key, a.ConnectionData, a.HostConnectionData);

            // Join game room as client
            NetworkManager.Singleton.StartClient();

            PeriodicallyRefreshLobby();

            return lobby;
        }
        catch (Exception e)
        {
            Debug.Log("No lobbies available via quick join");
            return null;
        }
    }

    private async static Task<Lobby> CreateLobby()
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

            // Set game room to use the relay allocation
            Transport.SetHostRelayData(a.RelayServer.IpV4, (ushort)a.RelayServer.Port, a.AllocationIdBytes, a.Key, a.ConnectionData);

            NetworkManager.Singleton.StartHost();

            // Kepp lobby alive
            Heartbeat();
            PeriodicallyRefreshLobby();

            return lobby;
        }
        catch (Exception e)
        {
            Debug.Log($"Failed to create a lobby. {e.Message}");
            return null;
        }
    }

    public static async Task LeaveLobby()
    {
        _heartbeatSource?.Cancel();
        _updateLobbySource?.Cancel();

        if (_connectedLobby != null)
        {
            try
            {
                if (_connectedLobby.HostId == MyAuthenticationService.PlayerId)
                {
                    await Lobbies.Instance.DeleteLobbyAsync(_connectedLobby.Id);
                    Debug.Log($"Delete lobby {_connectedLobby.Id}");
                }
                else
                {
                    await Lobbies.Instance.RemovePlayerAsync(_connectedLobby.Id, MyAuthenticationService.PlayerId);
                    Debug.Log($"Remove player from lobby {_connectedLobby.Id}");
                }
                _connectedLobby = null;
            }
            catch (Exception e)
            {
                Debug.Log($"Failed to leave lobby {_connectedLobby.Id}");
                Debug.Log(e);
            }
        }
    }

    public static async Task LockLobby()
    {
        try
        {
            await Lobbies.Instance.UpdateLobbyAsync(_connectedLobby.Id, new UpdateLobbyOptions() { IsLocked = true });
        }
        catch (Exception e)
        {
            Debug.Log($"Failed to lock lobby {_connectedLobby.Id}");
        }
    }

    private static async void Heartbeat()
    {
        _heartbeatSource = new CancellationTokenSource();
        while (!_heartbeatSource.IsCancellationRequested && _connectedLobby != null)
        {
            await Lobbies.Instance.SendHeartbeatPingAsync(_connectedLobby.Id);
            await Task.Delay(HeartbeatInterval * 1000);
        }
    }
    private static async void PeriodicallyRefreshLobby()
    {
        _updateLobbySource = new CancellationTokenSource();
        await Task.Delay(LobbyRefreshRate * 1000);
        while (!_updateLobbySource.IsCancellationRequested && _connectedLobby != null)
        {
            _connectedLobby = await Lobbies.Instance.GetLobbyAsync(_connectedLobby.Id);
            CurrentLobbyRefreshed?.Invoke(_connectedLobby);
            await Task.Delay(LobbyRefreshRate * 1000);
        }
    }
}
