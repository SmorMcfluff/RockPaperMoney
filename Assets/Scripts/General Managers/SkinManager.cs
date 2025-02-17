using System.Collections.Generic;
using UnityEngine;

public class SkinManager : MonoBehaviour
{
    public static SkinManager Instance;

    [SerializeField] public List<Skin> skins;


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
        }
    }


    public Skin GetSkin(SkinType skin)
    {
        return skins[(int)skin];
    }


    public Sprite[] GetIcons(SkinType skinType)
    {
        Skin skin = skins[(int)skinType];

        int lastPaperIndex = skin.playerAHandSprites.paperSprites.Length - 1;
        int lastScissorIndex = skin.playerAHandSprites.scissorSprites.Length - 1;

        //if(skin.skin == SkinType.Stickman)
        //{
        //    return new Sprite[0];
        //}

        return new Sprite[]
        {
            skin.playerAHandSprites.defaultHandSprite,
            skin.playerAHandSprites.paperSprites[lastPaperIndex],
            skin.playerAHandSprites.scissorSprites[lastScissorIndex]
        };
    }
}
