using UnityEngine;


[System.Serializable]
public class ArmSprites
{
    public Sprite upperArm;
    public Sprite lowerArm;
}

[System.Serializable]
public class HandSprites
{
    public Sprite defaultHandSprite;
    public Sprite[] paperSprites;
    public Sprite[] scissorSprites;
}


[CreateAssetMenu(fileName = "Skin", menuName = "Scriptable Objects/PurchasableObject/Skin")]
public class Skin : PurchasableObject
{
    public SkinType skin;

    public ArmSprites playerAArmSprites;
    public ArmSprites playerBArmSprites;

    public HandSprites playerAHandSprites;
    public HandSprites playerBHandSprites;


    public override void GetIcon()
    {
        var icons = SkinManager.Instance.GetIcons(skin);

        if(icons.Length > 0)
        {
            icon = icons[0];
        }
    }


    public override bool GetBought()
    {
        if(!base.GetBought())
        {
            return false;
        }

        SaveDataManager.Instance.localPlayerData.ownedSkins.Add(skin);
        return true;
    }
}

public enum SkinType
{
    Default,
    Pale,
    Light,
    Neutral,
    Tan,
    Dark,
    DefaultGloved,
    PaleGloved,
    LightGloved,
    NeutralGloved,
    TanGloved,
    DarkGloved,
    Stickman
}