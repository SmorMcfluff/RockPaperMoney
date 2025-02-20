using TMPro;
using UnityEngine;
using System.Collections.Generic;

public class StatsManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI winText;
    [SerializeField] private TextMeshProUGUI lossText;
    [SerializeField] private TextMeshProUGUI percentageText;
    [SerializeField] private TextMeshProUGUI adWatchersText;
    [SerializeField] private TextMeshProUGUI averageAgeText;
    [SerializeField] private TextMeshProUGUI medianAgeText;
    [SerializeField] private TextMeshProUGUI oldestEmployeeText;
    [SerializeField] private TextMeshProUGUI youngestEmployeeText;


    List<AdWatcherInfo> adWatchers = new();


    private void Awake()
    {
        PlayerData localPlayer = SaveDataManager.Instance.localPlayerData;

        if (localPlayer.factories.Count > 0)
        {
            adWatchers = GetSortedAdWatcherList(localPlayer);
        }

        winText.text = SetText("Wins", localPlayer.winCount);
        lossText.text = SetText("Losses", localPlayer.lossCount);
        percentageText.text = GetWinPercentage(localPlayer);

        adWatchersText.text = SetText("Ad Watchers", localPlayer.factories.Count);
        averageAgeText.text = GetAverageAge();
        medianAgeText.text = GetMedianAge();

        if (adWatchers.Count > 0)
        {
            oldestEmployeeText.text = GetName("Oldest Employee", adWatchers[^1]);
            youngestEmployeeText.text = GetName("Youngest Employee", adWatchers[0]);
        }
        else
        {
            oldestEmployeeText.text = SetText("Oldest Employee", "none");
            youngestEmployeeText.text = SetText("Youngest Employee", "none");
        }
    }


    private List<AdWatcherInfo> GetSortedAdWatcherList(PlayerData localPlayer)
    {
        adWatchers.Clear();
        foreach (var factory in localPlayer.factories)
        {
            adWatchers.Add(factory.adWatcherInfo);
        }
        adWatchers.Sort((a, b) => a.age.CompareTo(b.age));
        return adWatchers;
    }


    private string SetText(string title, object value, bool valueIsPercentage = false)
    {
        string str = $"{title}: {value}";
        if (valueIsPercentage)
        {
            str += "%";
        }

        return str;
    }


    private string GetWinPercentage(PlayerData localPlayer)
    {
        float totalGames = localPlayer.winCount + localPlayer.lossCount;

        if (totalGames == 0)
        {
            return ":)";
        }

        float winPercentage = (localPlayer.winCount / totalGames) * 100;
        winPercentage = Mathf.Round(winPercentage * 100f) / 100f;

        return SetText("Win Percentage", winPercentage, true);
    }


    private string GetAverageAge()
    {
        float adWatcherCount = adWatchers.Count;

        if (adWatcherCount == 0)
        {
            return ":)";
        }

        float totalAge = 0;

        foreach (var adWatcher in adWatchers)
        {
            totalAge += adWatcher.age;
        }

        float averageAge = Mathf.Round(totalAge / adWatcherCount * 100f) / 100f;

        return SetText("Average Age", averageAge);
    }


    private string GetMedianAge()
    {
        int adWatcherCount = adWatchers.Count;

        if (adWatcherCount == 0)
        {
            return ":)";
        }

        float medianAge;

        if (adWatcherCount % 2 == 0)
        {
            int midIndex = adWatcherCount / 2;

            medianAge = (adWatchers[midIndex - 1].age + adWatchers[midIndex].age) / 2.0f;
        }
        else
        {
            medianAge = adWatchers[adWatcherCount / 2].age;
        }

        medianAge = Mathf.Round(medianAge * 100f) / 100f;

        return SetText("Median Age", medianAge);
    }


    private string GetName(string title, AdWatcherInfo adWatcher)
    {
        string name = $"{adWatcher.firstName} {adWatcher.lastName}";

        return SetText(title, name);
    }
}