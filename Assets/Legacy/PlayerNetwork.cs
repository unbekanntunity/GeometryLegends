using Unity.Netcode;
using UnityEngine;

public class PlayerNetwork : NetworkBehaviour
{

    [SerializeField] private float MoveSpeed = 3f;

    private void Update()
    {
        if (!IsOwner)
        {
            return;
        }

        UpdateMovementServerRpc(OwnerClientId);
    }

    [ServerRpc]
    private void UpdateMovementServerRpc(ulong clientId)
    {
        var body = NetworkManager.ConnectedClients;

        var moveDir = new Vector3(0, 0, 0);

        if (Input.GetKey(KeyCode.W))
        {
            moveDir.z = 1f;
        }

        if (Input.GetKey(KeyCode.S))
        {
            moveDir.z = -1f;
        }

        if (Input.GetKey(KeyCode.A))
        {
            moveDir.x = -1f;
        }

        if (Input.GetKey(KeyCode.D))
        {
            moveDir.x = 1f;
        }

        transform.position += MoveSpeed * Time.deltaTime * moveDir;


        UpdateMovementClientRpc();
    }

    [ClientRpc]
    public void UpdateMovementClientRpc()
    {
        if (!IsOwner)
        {
            return;
        }

       
    }
}
