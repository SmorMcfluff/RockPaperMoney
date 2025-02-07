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


    public override void GetBought()
    {
        base.GetBought();
        SaveDataManager.Instance.localPlayerData.ownedSkins.Add(skin);
    }
}

public enum SkinType
{
    Undefined,
    Default
}
