using Assets.Scripts.Player;
using Unity.Netcode;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    public class GameManager : NetworkBehaviour
    {
        [SerializeField] private PlayerController _playerPrefab;

        public override void OnNetworkSpawn()
        {
            SpawnPlayerServerRpc(NetworkManager.Singleton.LocalClientId);
        }

        [ServerRpc(RequireOwnership = false)]
        private void SpawnPlayerServerRpc(ulong playerId)
        {
            var spawn = Instantiate(_playerPrefab);
            spawn.NetworkObject.SpawnWithOwnership(playerId);
        }

        public override async void OnDestroy()
        {
            base.OnDestroy();
            await MatchmakingService.LeaveLobby();
            if (NetworkManager.Singleton != null) NetworkManager.Singleton.Shutdown();
        }
    }
}
