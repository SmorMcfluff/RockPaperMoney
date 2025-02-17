using UnityEngine;

[CreateAssetMenu(fileName = "HandSignPurchasable", menuName = "Scriptable Objects/PurchasableObject/HandSignPurchasable")]
public class HandSignPurchasable : PurchasableObject
{
    public int unlockedHandSignIndex;
    // 0 = rock
    // 1 = paper
    // 2 = scissors

    public override void GetIcon()
    {
        SkinType equippedSkin = SaveDataManager.Instance.localPlayerData.equippedSkin;
        Sprite[] icons = SkinManager.Instance.GetIcons(equippedSkin);

        if (icons.Length > 0)
        {
            icon = unlockedHandSignIndex switch
            {
                0 => icons[0],
                1 => icons[1],
                2 => icons[2],
                _ => null
            };
        }
        else
        {
             icon = SkinManager.Instance.GetSkin(equippedSkin).icon;
        }
    }

    public override bool GetBought()
    {
        if (!base.GetBought())
        {
            return false;
        }

        SaveDataManager.Instance.localPlayerData.unlockedHandSigns[unlockedHandSignIndex] = true;
        return true;
    }
}