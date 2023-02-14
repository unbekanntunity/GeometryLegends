using System;
using UnityEngine;

namespace Assets.Scripts.Screens
{
    public class HomeScreen : MonoBehaviour
    {
        public static event Action SearchStarted;

        public void StartSearch()
        {
            SearchStarted?.Invoke();
        }
    }
}
