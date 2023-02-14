using Assets.Scripts.Screens;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyManager : NetworkBehaviour
{
    [SerializeField] private HomeScreen _homeScreen;
    [SerializeField] private SearchingLobbyScreen _searchScreen;
    [SerializeField] private LobbyFoundScreen _foundScreen;

    private static readonly Dictionary<ulong, bool> _playersInLobby = new();

    public static Action gameStarted;
    public static Action lobbyFound;

    public static int AmountOfPlayers
    {
        get
        {
            return _playersInLobby.Count;
        }

    }

    public static int AmountOfReadyPlayers
    {
        get
        {
            return _playersInLobby.Count(x => x.Value);
        }

    }


    private void Start()
    {
        _homeScreen.gameObject.SetActive(true);
        _searchScreen.gameObject.SetActive(false);
        _foundScreen.gameObject.SetActive(false);

        HomeScreen.SearchStarted += OnStartSearching;
        LobbyFoundScreen.ReadyPressed += OnReadyClicked;
        LobbyFoundScreen.LeaveLobbyPressed += OnLobbyLeaveClicked;

        NetworkObject.DestroyWithScene = true;
    }

    public static event Action<Dictionary<ulong, bool>> LobbyPlayersUpdated;

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnectedCallback;
            _playersInLobby.Add(NetworkManager.Singleton.LocalClientId, false);
            UpdateInterface();
        }

        // Client uses this in case host destroys the lobby
        NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnectCallback;
    }

    public async void OnStartSearching()
    {
        try
        {
            _homeScreen.gameObject.SetActive(false);
            _searchScreen.gameObject.SetActive(true);

            await MatchmakingService.CreateOrJoinLobby();

        }
        catch (Exception e)
        {
            Debug.Log("Failed to start search");
            CanvasUtilities.Instance.ShowError("Failed starting search");
        }
    }

    public async void OnLobbyLeaveClicked()
    {
        try
        {
            _foundScreen.gameObject.SetActive(false);
            _homeScreen.gameObject.SetActive(true);

            await MatchmakingService.LeaveLobby();

        }
        catch (Exception e)
        {
            Debug.Log("Failed to leave the lobby");
            CanvasUtilities.Instance.ShowError("Failed leaving lobby");
        }
    }

    private void OnClientConnectedCallback(ulong playerId)
    {
        if (!IsServer)
        {
            return;
        }

        if (!_playersInLobby.ContainsKey(playerId))
        {
            _playersInLobby.Add(playerId, false);
        }

        _searchScreen.gameObject.SetActive(false);
        _foundScreen.gameObject.SetActive(true);

        lobbyFound?.Invoke();

        PropagateToClients();

        UpdateInterface();
    }

    private void PropagateToClients()
    {
        foreach (var player in _playersInLobby) UpdatePlayerClientRpc(player.Key, player.Value);
    }

    [ClientRpc]
    private void UpdatePlayerClientRpc(ulong clientId, bool isReady)
    {
        if (IsServer) return;

        if (!_playersInLobby.ContainsKey(clientId)) _playersInLobby.Add(clientId, isReady);
        else _playersInLobby[clientId] = isReady;
        UpdateInterface();
    }

    private void OnClientDisconnectCallback(ulong playerId)
    {
        if (IsServer)
        {
            if (_playersInLobby.ContainsKey(playerId))
            {
                _playersInLobby.Remove(playerId);
            }

            // Propagate all clients
            RemovePlayerClientRpc(playerId);

            UpdateInterface();
        }
        else
        {
            // This happens when the host disconnects the lobby
            _foundScreen.gameObject.SetActive(false);
            _homeScreen.gameObject.SetActive(true);
            OnLobbyLeft();
        }
    }

    [ClientRpc]
    private void RemovePlayerClientRpc(ulong clientId)
    {
        if (IsServer)
        {
            return;
        }

        if (_playersInLobby.ContainsKey(clientId))
        {
            _playersInLobby.Remove(clientId);
        }

        UpdateInterface();
    }

    public void OnReadyClicked()
    {
        SetReadyServerRpc(NetworkManager.Singleton.LocalClientId);
    }

    [ServerRpc(RequireOwnership = false)]
    private void SetReadyServerRpc(ulong playerId)
    {
        _playersInLobby[playerId] = true;

        if (_playersInLobby.All(x => x.Value))
        {
            StartGame();
        }

        PropagateToClients();
        UpdateInterface();
    }

    private void UpdateInterface()
    {
        LobbyPlayersUpdated?.Invoke(_playersInLobby);
    }

    private async void OnLobbyLeft()
    {
        _playersInLobby.Clear();
        NetworkManager.Singleton.Shutdown();
        await MatchmakingService.LeaveLobby();
    }

    public override void OnDestroy()
    {
        base.OnDestroy();

        // We only care about this during lobby
        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.OnClientDisconnectCallback -= OnClientDisconnectCallback;
        }
    }
    private async void StartGame()
    {
        await MatchmakingService.LockLobby();
        NetworkManager.Singleton.SceneManager.LoadScene("Battlefield", LoadSceneMode.Single);

        gameStarted?.Invoke();
        Debug.Log("Game started");
    }
}
