using System.Collections;
using UnityEngine;

public class RPSViewHand : MonoBehaviour
{
    [SerializeField] SpriteRenderer upperArmSr;
    [SerializeField] SpriteRenderer lowerArmSr;
    [SerializeField] SpriteRenderer handSr;

    [SerializeField] Sprite[] paperSprites;
    [SerializeField] Sprite[] scissorSprites;

    [SerializeField] PlayerLetter playerLetter;

    float animDuration = 0.25f;

    private void Awake()
    {
        SetSkin();
    }

    public void SetSkin()
    {
        SkinType equippedSkin = playerLetter == PlayerLetter.A
            ? RPSMatchMaking.Instance.gameData.playerA.equippedSkin
            : RPSMatchMaking.Instance.gameData.playerB.equippedSkin;

        Skin skin = SkinManager.Instance.GetSkin(equippedSkin);

        ArmSprites armSprites = (playerLetter == PlayerLetter.A) ? skin.playerAArmSprites : skin.playerBArmSprites;
        HandSprites handSprites = (playerLetter == PlayerLetter.A) ? skin.playerAHandSprites : skin.playerBHandSprites;

        upperArmSr.sprite = armSprites.upperArm;
        lowerArmSr.sprite = armSprites.lowerArm;

        handSr.sprite = handSprites.defaultHandSprite;
        paperSprites = handSprites.paperSprites;
        scissorSprites = handSprites.scissorSprites;
    }


    public void SetAnimation() // Gets called from an animation event
    {
        Debug.Log("Anim set");
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
                handSr.enabled = false;
                break;
        }
    }


    private IEnumerator PlayPaper()
    {
        int frameAmount = paperSprites.Length;
        float frameDuration = animDuration / frameAmount;

        for (int i = 0; i < frameAmount; i++)
        {
            handSr.sprite = paperSprites[i];
            yield return new WaitForSecondsRealtime(frameDuration);
        }
    }


    private IEnumerator PlayScissors()
    {
        int frameAmount = scissorSprites.Length;
        float frameDuration = animDuration / frameAmount;

        for (int i = 0; i < frameAmount; i++)
        {
            handSr.sprite = scissorSprites[i];
            yield return new WaitForSecondsRealtime(frameDuration);
        }
    }
}

enum PlayerLetter
{
    A, B
}
