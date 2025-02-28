using System;
using UnityEngine;

public class AntiTimeCheat : MonoBehaviour
{
    public static AntiTimeCheat Instance;
    [SerializeField] GameObject cheatDetectedBox;

    TimeSpan acceptableRange = TimeSpan.FromHours(2);

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            InternetChecker.IsInternetAvailable(() =>
            {
                if (CheckForTimeCheat())
                {
                    Instantiate(cheatDetectedBox, FindFirstObjectByType<Canvas>().transform);
                    Destroy(gameObject);
                }
            });
        }
    }


    private void OnApplicationFocus(bool focus)
    {
        if (!focus)
        {
            return;
        }

        InternetChecker.IsInternetAvailable(() =>
        {
            if (CheckForTimeCheat())
            {
                Instantiate(cheatDetectedBox, FindFirstObjectByType<Canvas>().transform);
                Destroy(gameObject);
            }
        });
    }


    private bool CheckForTimeCheat()
    {
        var networkTime = NTPTimeFetcher.GetNetworkTime();
        var systemTime = DateTime.UtcNow;

        var timeDifference = (networkTime - systemTime).Duration();

        return (timeDifference > acceptableRange);
    }
}
