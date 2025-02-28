using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    private List<TutorialMessage> currentMessageList;
    [SerializeField] private List<TutorialMessage> preFirstGameTutorialMessages = new();
    [SerializeField] private List<TutorialMessage> preFirstAdMessages = new();
    [SerializeField] private List<TutorialMessage> preSecondAdMessages = new();
    [SerializeField] private List<TutorialMessage> preFactoryBoughtMessages = new();
    [SerializeField] private List<TutorialMessage> finalMessage = new();

    [SerializeField] private Button nextButton;

    [SerializeField] private GameObject textPanel;
    [SerializeField] private TextMeshProUGUI messageText;

    private int messageIndex = 0;

    private void Awake()
    {
        if (!SaveDataManager.Instance.localPlayerData.hasFinishedTutorial)
        {
            currentMessageList = CheckTutorialPart();
            StartTutorial();
        }
    }


    public List<TutorialMessage> CheckTutorialPart()
    {
        PlayerData localPlayer = SaveDataManager.Instance.localPlayerData;

        if (!localPlayer.hasPlayedFirstGame)
        {
            return preFirstGameTutorialMessages;
        }
        else if (localPlayer.watchedAdCount == 0)
        {
            return preFirstAdMessages;
        }
        else if (localPlayer.watchedAdCount == 1)
        {
            return preSecondAdMessages;
        }
        else if (localPlayer.factories.Count < 1)
        {
            return preFactoryBoughtMessages;
        }
        return finalMessage;
    }


    public void StartTutorial()
    {
        textPanel.SetActive(true);
        messageText.text = currentMessageList[messageIndex].message;
    }


    public void NextMessage()
    {
        messageIndex++;
        if (messageIndex < currentMessageList.Count)
        {
            messageText.text = currentMessageList[messageIndex].message;
        }
        else
        {
            if (currentMessageList == finalMessage)
            {
                SaveDataManager.Instance.localPlayerData.hasFinishedTutorial = true;
            }
            textPanel.SetActive(false);
        }
    }
}
