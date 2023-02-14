using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Screens
{
    public class LobbyFoundScreen : MonoBehaviour
    {
        [SerializeField] private TMP_Text _playersJoinedText;
        [SerializeField] private Button _readyBtn;

        private bool _isReady = false;
        private TMP_Text _text;

        public static Action ReadyPressed;
        public static Action LeaveLobbyPressed;

        private void Start()
        {
            _text = _readyBtn.GetComponentInChildren<TMP_Text>();
        }

        private void Update()
        {
            _playersJoinedText.text = $"{LobbyManager.AmountOfReadyPlayers} / {LobbyManager.AmountOfPlayers}";
        }

        public void ToggleReadyState()
        {
            _isReady = !_isReady;

            _text.text = _isReady ? "Cancel" : "Get ready";

            ReadyPressed?.Invoke();
        }

        public void LeaveLobby()
        {
            LeaveLobbyPressed?.Invoke();
        }
    }
}
