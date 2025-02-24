using TMPro;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;

public class StatsManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI winText;
    [SerializeField] private TextMeshProUGUI lossText;
    [SerializeField] private TextMeshProUGUI percentageText;

    [SerializeField] private TextMeshProUGUI adWatchersText;
    [SerializeField] private TextMeshProUGUI averageAgeText;
    [SerializeField] private TextMeshProUGUI medianAgeText;
    [SerializeField] private TextMeshProUGUI modeAgeText;

    [SerializeField] private TextMeshProUGUI oldestEmployeeText;
    [SerializeField] private TextMeshProUGUI youngestEmployeeText;

    [SerializeField] private TextMeshProUGUI mTFRatioText;

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

        adWatchersText.text = SetText("AdVideo Watchers", localPlayer.factories.Count);
        averageAgeText.text = GetAverageAge();
        medianAgeText.text = GetMedianAge();
        modeAgeText.text = GetModeAges();

        if (adWatchers.Count > 0)
        {
            oldestEmployeeText.text = GetNameAndAge("Oldest Employee", adWatchers[^1]);
            youngestEmployeeText.text = GetNameAndAge("Youngest Employee", adWatchers[0]);
        }
        else
        {
            oldestEmployeeText.text = SetText("Oldest Employee", ":)");
            youngestEmployeeText.text = SetText("Youngest Employee", ":)");
        }

        mTFRatioText.text = GetMaleToFemaleRatio();
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
        string title = "Win Percentage";

        float totalGames = localPlayer.winCount + localPlayer.lossCount;
        if (totalGames == 0)
        {
            return GetEmptyStatText(title);
        }

        float winPercentage = (localPlayer.winCount / totalGames) * 100;
        winPercentage = MathF.Round(winPercentage, 2);

        return SetText(title, winPercentage, true);
    }


    private string GetAverageAge()
    {
        string title = "Average Age";

        float adWatcherCount = adWatchers.Count;
        if (adWatcherCount == 0)
        {
            return GetEmptyStatText(title);
        }

        float totalAge = 0;

        foreach (var adWatcher in adWatchers)
        {
            totalAge += adWatcher.age;
        }

        float averageAge = MathF.Round(totalAge / adWatcherCount, 2);

        return SetText(title, averageAge);
    }


    private string GetMedianAge()
    {
        string title = "Median Age";

        int adWatcherCount = adWatchers.Count;
        if (adWatcherCount == 0)
        {
            return GetEmptyStatText(title);
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

        medianAge = MathF.Round(medianAge, 2);

        return SetText(title, medianAge);
    }


    private string GetModeAges()
    {
        string title = "Most Common Age";

        if (adWatchers.Count == 0)
        {
            return GetEmptyStatText(title);
        }

        Dictionary<int, int> ageFrequencies = new();
        foreach (var adWatcher in adWatchers)
        {
            if (ageFrequencies.ContainsKey(adWatcher.age))
            {
                ageFrequencies[adWatcher.age]++;
            }
            else
            {
                ageFrequencies[adWatcher.age] = 1;
            }
        }

        int maxFrequency = ageFrequencies.Values.Max();

        if (maxFrequency == 1)
        {
            return SetText(title, "None");
        }

        var modes = ageFrequencies.Where(pair => pair.Value == maxFrequency).Select(pair => pair.Key).ToList();

        string modeText = string.Join(", ", modes);

        if (modes.Count != 1)
        {
            title += "s";
        }

        return SetText(title, modeText);
    }


    private string GetNameAndAge(string title, AdWatcherInfo adWatcher)
    {
        string nameAndAge = $"{adWatcher.firstName} {adWatcher.lastName} ({adWatcher.age})";

        return SetText(title, nameAndAge);
    }


    private string GetMaleToFemaleRatio()
    {
        string title = "M/F Ratio";

        int maleCount = adWatchers.Count(adWatcher => adWatcher.sex == Sex.Male);
        int femaleCount = adWatchers.Count(adWatcher => adWatcher.sex == Sex.Female);

        if(femaleCount == 0)
        {
            return SetText(title, "Sausage Party");
        }
        else if(maleCount == 0)
        {
            return SetText(title, "Taco Night");
        }
        float ratio = MathF.Round((float)maleCount / femaleCount, 2);

        return SetText(title, ratio.ToString("0.00"));
    }


    private string GetEmptyStatText(string title)
    {
        return SetText(title, ":)");
    }
}