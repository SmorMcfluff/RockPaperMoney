using System.Collections;
using UnityEngine;

public class PreviewManager : MonoBehaviour
{
    public static PreviewManager Instance;

    [SerializeField] public GameObject previewScreen;

    [SerializeField] private Transform playerAArm;
    [SerializeField] private Transform playerBArm;

    [SerializeField] private SpriteRenderer playerAUpperArm;
    [SerializeField] private SpriteRenderer playerALowerArm;
    [SerializeField] private SpriteRenderer playerAHand;

    [SerializeField] private SpriteRenderer playerBUpperArm;
    [SerializeField] private SpriteRenderer playerBLowerArm;
    [SerializeField] private SpriteRenderer playerBHand;

    private Sprite[] playerAHandSprites;
    private Sprite[] playerBHandSprites;

    public bool previewIsActive;


    void Awake()
    {
        Instance = this;
        AlignArmToScreen();
    }


    private void AlignArmToScreen()
    {
        Camera cam = Camera.main;
        float zDepth = Mathf.Abs(cam.transform.position.z - playerAArm.position.z);

        float yPos = cam.WorldToScreenPoint(playerAArm.position).y;

        Vector3 playerAPos = cam.ScreenToWorldPoint(new Vector3(0, yPos, zDepth));
        Vector3 playerBPos = cam.ScreenToWorldPoint(new Vector3(Screen.width, yPos, zDepth));

        playerAArm.position = playerAPos;
        playerBArm.position = playerBPos;

        AdjustArmScale(zDepth, cam);
    }


    private void AdjustArmScale(float zDepth, Camera cam)
    {
        float screenWidth = cam.ScreenToWorldPoint(new Vector3(Screen.width, 0, zDepth)).x;

        float targetWidth = screenWidth / 2;

        float armAWidth = GetArmWidth(playerAArm);
        float armBWidth = GetArmWidth(playerBArm);

        float scaleA = targetWidth / armAWidth;
        float scaleB = targetWidth / armBWidth;

        playerAArm.localScale = new(scaleA, scaleA, 1);
        playerBArm.localScale = new(scaleB, scaleB, 1);
    }


    private float GetArmWidth(Transform arm)
    {
        SpriteRenderer[] sprites = arm.GetComponentsInChildren<SpriteRenderer>();
        if (sprites.Length == 0) return 1f;

        float leftX = float.MaxValue;
        float rightX = float.MinValue;

        foreach (var sprite in sprites)
        {
            Bounds bounds = sprite.bounds;
            leftX = Mathf.Min(leftX, bounds.min.x);
            rightX = Mathf.Max(rightX, bounds.max.x);
        }

        float width = rightX - leftX;
        return Mathf.Max(width, 1f);
    }


    private IEnumerator CycleHandSigns()
    {
        while (previewIsActive)
        {
            for (int i = 0; i < 3; i++)
            {
                playerAHand.sprite = playerAHandSprites[i];
                playerBHand.sprite = playerBHandSprites[i];

                yield return new WaitForSeconds(0.5f);
            }
            yield return null;
        }
        yield break;
    }


    public void ActivatePreview(SkinType skinToPreview)
    {
        previewIsActive = true;

        SetSkins(skinToPreview);
        previewScreen.SetActive(true);
        StoreManager.Instance.storePanel.SetActive(false);
        StartCoroutine(CycleHandSigns());
    }

    public void DeactivatePreview()
    {
        previewIsActive = false;
        previewScreen.SetActive(false);
        StoreManager.Instance.storePanel.SetActive(true);
    }


    public void SetSkins(SkinType skinToPreview)
    {
        Skin skin = SkinManager.Instance.GetSkin(skinToPreview);
        SetPlayerASkin(skin);
        SetPlayerBSkin(skin);
    }


    private void SetPlayerASkin(Skin skin)
    {
        int lastPaperIndex = skin.playerAHandSprites.paperSprites.Length - 1;
        int lastScissorIndex = skin.playerAHandSprites.scissorSprites.Length - 1;

        playerAUpperArm.sprite = skin.playerAArmSprites.upperArm;
        playerALowerArm.sprite = skin.playerAArmSprites.lowerArm;

        playerAHandSprites = new Sprite[]
        {

            skin.playerAHandSprites.defaultHandSprite,
            skin.playerAHandSprites.paperSprites[lastPaperIndex],
            skin.playerAHandSprites.scissorSprites[lastScissorIndex],
        };
    }

    private void SetPlayerBSkin(Skin skin)
    {
        int lastPaperIndex = skin.playerAHandSprites.paperSprites.Length - 1;
        int lastScissorIndex = skin.playerAHandSprites.scissorSprites.Length - 1;

        playerBUpperArm.sprite = skin.playerBArmSprites.upperArm;
        playerBLowerArm.sprite = skin.playerBArmSprites.lowerArm;
        playerBHandSprites = new Sprite[]
        {
            skin.playerBHandSprites.defaultHandSprite,
            skin.playerBHandSprites.paperSprites[lastPaperIndex],
            skin.playerBHandSprites.scissorSprites[lastScissorIndex],
        };
    }
}
