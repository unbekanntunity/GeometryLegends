using Unity.Netcode;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PlayerController : NetworkBehaviour

    {
        [SerializeField] private float _moveSpeed = 3;

        private void Update()
        {
            if(!IsOwner)
            {
                return;
            }

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

            transform.position += _moveSpeed * Time.deltaTime * moveDir;
        }

        public override void OnNetworkSpawn()
        {
            if (!IsOwner) Destroy(this);
        }
    }
}
