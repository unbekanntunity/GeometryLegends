using Assets.Scripts.Services;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AuthenticationManager : MonoBehaviour
{
    public async void LoginAnonymously()
    {
        await MyAuthenticationService.Login();
        SceneManager.LoadSceneAsync("Home");
    }
}
