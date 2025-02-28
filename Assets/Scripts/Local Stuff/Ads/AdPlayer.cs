using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

[RequireComponent(typeof(VideoPlayer))]
public class AdPlayer : MonoBehaviour
{
    private static float defaultAdMoney = 0.05f;
    private static float defaultMinViewTime = 15f;

    private VideoPlayer videoPlayer;
    [SerializeField] private AdVideo[] adVideos;

    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private Button closeButton;

    private AdVideo currentAd;

    private void Awake()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        currentAd = SetAdVideo();
        PlayAd();
    }


    private AdVideo SetAdVideo()
    {
        return adVideos[Random.Range(0, adVideos.Length)];
    }


    private void PlayAd()
    {
        videoPlayer.clip = currentAd.clip;
        videoPlayer.Play();
        videoPlayer.playbackSpeed = GetPlaybackSpeed();
        videoPlayer.loopPointReached += StopPlaying;

        StartCoroutine(CountDownAd());
    }


    private float GetPlaybackSpeed()
    {
        float minViewTime = GetMinViewTime();
        float normalizedPlaybackSpeed = 1 + ((defaultMinViewTime - minViewTime) / defaultMinViewTime) * 4;
        normalizedPlaybackSpeed = Mathf.Clamp(normalizedPlaybackSpeed, 1, 5);

        return normalizedPlaybackSpeed;
    }


    private IEnumerator CountDownAd()
    {
        float minViewTime = GetMinViewTime();
        float elapsedTime = 0;

        while (elapsedTime < minViewTime)
        {
            elapsedTime += Time.deltaTime;

            SetTimerText(elapsedTime);
            yield return null;
        }
        closeButton.interactable = true;
        timerText.text = "X";
        closeButton.image.color = closeButton.image.color.SetAlpha(1);
        SaveDataManager.Instance.localPlayerData.ChangeMoneyBalance(GetAdMoneyAmount());
    }


    private void SetTimerText(float elapsedTime)
    {
        float remainingTime = Mathf.Ceil(GetMinViewTime() - elapsedTime);

        if (remainingTime < 0)
        {
            return;
        }

        if (timerText.text != remainingTime.ToString())
        {
            timerText.text = remainingTime.ToString();
        }
    }


    public void OpenURL()
    {
        Application.OpenURL(currentAd.url);
    }


    public void StopPlaying(VideoPlayer videoPlayer = null)
    {
        videoPlayer.Stop();

        if (SaveDataManager.Instance.localPlayerData.watchedAdCount < 2)
        {
            SaveDataManager.Instance.localPlayerData.watchedAdCount++;
            InternetChecker.IsInternetAvailable(() => SaveDataManager.Instance.SavePlayer());
        }

        SceneController.Instance.ReturnToLastScene();
    }


    public static float GetMinViewTime()
    {
        return defaultMinViewTime - SaveDataManager.Instance.localPlayerData.timeUpgrades;
    }


    public static float GetAdMoneyAmount()
    {
        return defaultAdMoney * Mathf.Pow(2, SaveDataManager.Instance.localPlayerData.moneyUpgrades);
    }
}
