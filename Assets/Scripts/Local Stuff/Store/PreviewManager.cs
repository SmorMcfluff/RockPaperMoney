using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PreviewManager : MonoBehaviour
{
    public static PreviewManager Instance;

    public GameObject previewScreen;
    [SerializeField] private Button endPreviewButton;
    [SerializeField] private Button backButton;

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
        SetArmVisibilty(false);
    }


    private void AlignArmToScreen()
    {
        Camera cam = Camera.main;
        float zDepth = Mathf.Abs(cam.transform.position.z - playerAArm.position.z);

        float playerAY = cam.WorldToScreenPoint(playerAArm.position).y;
        float playerBY = cam.WorldToScreenPoint(playerBArm.position).y;


        Vector3 playerAPos = cam.ScreenToWorldPoint(new Vector3(0, playerAY, zDepth));
        Vector3 playerBPos = cam.ScreenToWorldPoint(new Vector3(Screen.width, playerBY, zDepth));

        playerAArm.position = playerAPos;
        playerBArm.position = playerBPos;

        AdjustArmScale(zDepth, cam);
    }


    private void AdjustArmScale(float zDepth, Camera cam)
    {
        float screenWidth = cam.ScreenToWorldPoint(new Vector3(Screen.width, 0, zDepth)).x;

        float targetWidth = screenWidth / 3;

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
        }
    }


    public void ActivatePreview(SkinType skinToPreview)
    {
        previewIsActive = true;
        SetArmVisibilty(previewIsActive);

        SetSkins(skinToPreview);
        endPreviewButton.gameObject.SetActive(true);
        backButton.gameObject.SetActive(false);

        if (skinToPreview != SkinType.Stickman)
        {
            StartCoroutine(CycleHandSigns());
        }

        if (StoreManager.Instance != null)
        {
            StoreManager.Instance.storePanel.SetActive(false);
        }
        else if (SkinViewer.Instance != null)
        {
            SkinViewer.Instance.skinViewPanel.SetActive(false);
        }
    }


    public void DeactivatePreview()
    {
        previewIsActive = false;
        endPreviewButton.gameObject.SetActive(false);
        backButton.gameObject.SetActive(true);

        SetArmVisibilty(previewIsActive);
        StopCoroutine(CycleHandSigns());

        if (StoreManager.Instance != null)
        {
            StoreManager.Instance.GenerateStoreItems();
            StoreManager.Instance.storePanel.SetActive(true);
        }
        else if (SkinViewer.Instance != null)
        {
            SkinViewer.Instance.skinViewPanel.SetActive(true);
        }

    }


    private void SetArmVisibilty(bool status)
    {
        playerAUpperArm.enabled = status;
        playerALowerArm.enabled = status;
        playerAHand.enabled = status;

        playerBUpperArm.enabled = status;
        playerBLowerArm.enabled = status;
        playerBHand.enabled = status;
    }


    public void SetSkins(SkinType skinToPreview)
    {
        Skin skin = SkinManager.Instance.GetSkin(skinToPreview);
        SetPlayerASkin(skin);
        SetPlayerBSkin(skin);
    }


    private void SetPlayerASkin(Skin skin)
    {
        playerAUpperArm.sprite = skin.playerAArmSprites.upperArm;
        playerALowerArm.sprite = skin.playerAArmSprites.lowerArm;

        if (skin.skin == SkinType.Stickman)
        {
            playerAHand.enabled = false;
            return;
        }
        else if (!playerAHand.enabled)
        {
            playerAHand.enabled = true;
        }

        int lastPaperIndex = skin.playerAHandSprites.paperSprites.Length - 1;
        int lastScissorIndex = skin.playerAHandSprites.scissorSprites.Length - 1;


        playerAHandSprites = new Sprite[]
        {
            skin.playerAHandSprites.defaultHandSprite,
            skin.playerAHandSprites.paperSprites[lastPaperIndex],
            skin.playerAHandSprites.scissorSprites[lastScissorIndex],
        };

    }


    private void SetPlayerBSkin(Skin skin)
    {
        playerBUpperArm.sprite = skin.playerBArmSprites.upperArm;
        playerBLowerArm.sprite = skin.playerBArmSprites.lowerArm;

        if (skin.skin == SkinType.Stickman)
        {
            playerBHand.enabled = false;
            return;
        }
        else if (!playerBHand.enabled)
        {
            playerBHand.enabled = true;
        }

        int lastPaperIndex = skin.playerBHandSprites.paperSprites.Length - 1;
        int lastScissorIndex = skin.playerBHandSprites.scissorSprites.Length - 1;


        playerBHandSprites = new Sprite[]
        {
            skin.playerBHandSprites.defaultHandSprite,
            skin.playerBHandSprites.paperSprites[lastPaperIndex],
            skin.playerBHandSprites.scissorSprites[lastScissorIndex],
        };
    }
}
