using ParrelSync;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;

namespace Assets.Scripts.Services
{
    public static class MyAuthenticationService
    {
        public static string PlayerId { get; private set; }

        public static async Task Login()
        {
            if (UnityServices.State == ServicesInitializationState.Uninitialized)
            {
                var options = new InitializationOptions();


#if UNITY_EDITOR
                if (ClonesManager.IsClone()) options.SetProfile(ClonesManager.GetArgument());
                else options.SetProfile("Primary");
#endif

                await UnityServices.InitializeAsync(options);
            }

            if (!AuthenticationService.Instance.IsSignedIn)
            {
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
                PlayerId = AuthenticationService.Instance.PlayerId;
            }
        }
    }
}
