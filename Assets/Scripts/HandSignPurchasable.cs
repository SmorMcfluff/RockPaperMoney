using UnityEngine;

[CreateAssetMenu(fileName = "HandSignPurchasable", menuName = "Scriptable Objects/PurchasableObject/HandSignPurchasable")]
public class HandSignPurchasable : PurchasableObject
{
    public int unlockedHandSignIndex;
    // 0 = rock
    // 1 = paper
    // 2 = scissors

    public void GetIcon()
    {
        SkinType equippedSkin = SaveDataManager.Instance.localPlayerData.equippedSkin;
        Skin skin = SkinManager.Instance.GetSkin(equippedSkin);

        icon = unlockedHandSignIndex switch
        {
            0 => skin.playerAHandSprites.defaultHandSprite,
            1 => skin.playerAHandSprites.paperSprites[2],
            2 => skin.playerAHandSprites.scissorSprites[1],
            _ => null
        };
    }

    public override void GetBought()
    {
        base.GetBought();

        SaveDataManager.Instance.localPlayerData.unlockedHandSigns[unlockedHandSignIndex] = true;
    }
}
