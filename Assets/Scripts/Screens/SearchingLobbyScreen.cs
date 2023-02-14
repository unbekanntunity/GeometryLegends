using System.Collections;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.Screens
{
    public class SearchingLobbyScreen : MonoBehaviour
    {
        [SerializeField] private TMP_Text _timer;

        private void Start()
        {
            StartCoroutine(StartTimerCoroutine());

            LobbyManager.lobbyFound += StopAllCoroutines;
        }

        private void OnEnable()
        {
            _timer.text = "0";
        }

        private void OnDestroy()
        {
            LobbyManager.lobbyFound -= StopAllCoroutines;
        }

        private IEnumerator StartTimerCoroutine()
        {
            var delay = new WaitForSecondsRealtime(1);

            while (true)
            {
                var timerValue = int.Parse(_timer.text);
                timerValue++;
                _timer.text = timerValue.ToString();
                yield return delay;
            }
        }
    }
}
