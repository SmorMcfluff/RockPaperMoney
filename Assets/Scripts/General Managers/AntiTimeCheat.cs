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

            if (CheckForTimeCheat())
            {
                Instantiate(cheatDetectedBox, FindAnyObjectByType<Canvas>().transform);
                Destroy(gameObject);
            }
        }
    }

    private void OnApplicationFocus(bool focus)
    {
        if (CheckForTimeCheat())
        {
            Instantiate(cheatDetectedBox, FindAnyObjectByType<Canvas>().transform);
            Destroy(gameObject);
        }
    }

    private bool CheckForTimeCheat()
    {
        var networkTime = NTPTimeFetcher.GetNetworkTime();
        var systemTime = DateTime.UtcNow;

        var timeDifference = (networkTime - systemTime).Duration();

        return (timeDifference > acceptableRange);
    }
}
