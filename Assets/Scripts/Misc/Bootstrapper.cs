using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Assets.Scripts.Misc
{
    public static class Bootstrapper
    {
        // Run once before every any other scene script
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize()
        {
            MatchmakingService.ResetStatics();
            Addressables.InstantiateAsync("CanvasUtilities");
        }
    }
}
