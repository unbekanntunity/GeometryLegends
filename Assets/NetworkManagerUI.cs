using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class NetworkManagerUI : MonoBehaviour
{
    [SerializeField] private Button HostBtn;
    [SerializeField] private Button ServerBtn;
    [SerializeField] private Button ClientBtn;

    private void Awake()
    {
        HostBtn.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartHost();
        });
        ServerBtn.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartServer();
        });
        ClientBtn.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartClient();
        });
    }
}
