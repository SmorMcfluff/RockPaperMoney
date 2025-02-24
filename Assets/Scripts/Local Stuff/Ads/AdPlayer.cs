using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

[RequireComponent(typeof(VideoPlayer))]
public class AdPlayer : MonoBehaviour
{
    private VideoPlayer player;
    [SerializeField] private AdVideo[] adVideos;

    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private Button closeButton;
    [SerializeField] private Button openURLButton;

    private AdVideo currentAd;
    private float minViewTime = 15f;

    private void Awake()
    {
        player = GetComponent<VideoPlayer>();
        currentAd = SetAdVideo();
        PlayAd();
    }


    private AdVideo SetAdVideo()
    {
        return adVideos[Random.Range(0, adVideos.Length)];
    }


    private void PlayAd()
    {
        player.clip = currentAd.clip;
        player.Play();

        StartCoroutine(CountDownAd());
    }


    public void OpenURL()
    {
        Application.OpenURL(currentAd.url);
    }
    
    
    private IEnumerator CountDownAd()
    {
        float elapsedTime = 0;

        while (elapsedTime < minViewTime)
        {
            elapsedTime += Time.deltaTime;

            SetTimerText(elapsedTime);
            yield return null;
        }
        timerText.text = "X";
    }


    private void SetTimerText(float elapsedTime)
    {
        float remainingTime = Mathf.Ceil(minViewTime - elapsedTime);

        if (timerText.text != remainingTime.ToString())
        {
            timerText.text = remainingTime.ToString();
        }
    }
}
