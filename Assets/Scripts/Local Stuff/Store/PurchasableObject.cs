using UnityEngine;

public abstract class PurchasableObject : ScriptableObject
{
    public string itemName;
    public float price;

    public Sprite icon;

    public virtual void GetBought()
    {
        PlayerData player = SaveDataManager.Instance.localPlayerData;

        if (!IsAffordable(player))
        {
            return;
        }
        else
        {
            player.ChangeMoneyBalance(-price);
        }
    }

    public bool IsAffordable(PlayerData player)
    {
        return (player.GetMoneyBalance() >= price);
    }
}