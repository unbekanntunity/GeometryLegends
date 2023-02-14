using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class CanvasUtilities : MonoBehaviour
{
    public static CanvasUtilities Instance;

    [SerializeField] private CanvasGroup _loader;
    [SerializeField] private TMP_Text _loaderText, _errorText;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
        Toggle(false);
    }

    public void Toggle(bool on, string text = null)
    {
        _loaderText.text = text;
        _loader.gameObject.SetActive(on);
    }

    public void ShowError(string error)
    {
        _errorText.text = error;
        StartCoroutine(StartFadingCorutine(() =>
        {
            _errorText.text = "";
        }));
    }

    public IEnumerator StartFadingCorutine(Action action)
    {
        yield return new WaitForSeconds(5);

        action.Invoke();
    }
}

public class Load : IDisposable
{
    public Load(string text)
    {
        CanvasUtilities.Instance.Toggle(true, text);
    }

    public void Dispose()
    {
        CanvasUtilities.Instance.Toggle(false);
    }
}