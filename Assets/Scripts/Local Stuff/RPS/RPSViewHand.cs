using System.Collections;
using UnityEngine;

public class RPSViewHand : MonoBehaviour
{
    [SerializeField] SpriteRenderer sr;

    [SerializeField] Sprite[] paperSprites;
    [SerializeField] Sprite[] scissorSprites;

    [SerializeField] PlayerLetter playerLetter;

    float animDuration = 0.25f;


    public void SetAnimation()
    {
        HandSign handSign = (playerLetter == PlayerLetter.A)
            ? RPSMatchMaking.Instance.gameData.playerA.handSign
            : RPSMatchMaking.Instance.gameData.playerB.handSign;

        if (handSign == HandSign.Rock) return;

        PlayHandAnimation(handSign);
    }


    private void PlayHandAnimation(HandSign handSign)
    {
        switch (handSign)
        {
            case HandSign.Paper:
                StartCoroutine(PlayPaper());
                break;
            case HandSign.Scissors:
                StartCoroutine(PlayScissors());
                break;
            default:
                sr.enabled = false;
                break;
        }
    }


    private IEnumerator PlayPaper()
    {
        int frameAmount = paperSprites.Length;
        float frameDuration = animDuration / frameAmount;

        for (int i = 0; i < frameAmount; i++)
        {
            sr.sprite = paperSprites[i];
            yield return new WaitForSecondsRealtime(frameDuration);
        }
    }


    private IEnumerator PlayScissors()
    {
        int frameAmount = scissorSprites.Length;
        float frameDuration = animDuration / frameAmount;

        for (int i = 0; i < frameAmount; i++)
        {
            sr.sprite = scissorSprites[i];
            yield return new WaitForSecondsRealtime(frameDuration);
        }
    }
}

enum PlayerLetter
{
    A, B
}
