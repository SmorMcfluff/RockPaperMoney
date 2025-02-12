using UnityEngine;

public abstract class PurchasableObject : ScriptableObject
{
    public string itemName;
    public float price;

    public Sprite icon;

    public abstract void GetIcon();


    public virtual bool GetBought()
    {
        PlayerData player = SaveDataManager.Instance.localPlayerData;

        if (!IsAffordable(player))
        {
            return false;
        }
        else
        {
            player.ChangeMoneyBalance(-price);
            return true;
        }
    }


    public bool IsAffordable(PlayerData player)
    {
        return (player.GetMoneyBalance() >= price);
    }
}