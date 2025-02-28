using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class InternetChecker : MonoBehaviour
{
    public static InternetChecker Instance;

    [SerializeField] private GameObject noInternetPanel;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public static void IsInternetAvailable(Action onSuccess)
    {
        Instance.StartCoroutine(Instance.CheckInternet(onSuccess));
    }


    private IEnumerator CheckInternet(Action onSuccess)
    {
        UnityWebRequest www = UnityWebRequest.Get("https://www.google.com");
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            onSuccess?.Invoke();
        }
        else
        {
            ShowInternetPanel();
        }
    }


    public void ShowInternetPanel()
    {
        Instantiate(noInternetPanel, FindFirstObjectByType<Canvas>().transform);
    }
}
