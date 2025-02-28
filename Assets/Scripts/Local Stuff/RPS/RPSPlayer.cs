[System.Serializable]
public class RPSPlayer
{
    public PlayerData playerData;

    public HandSign handSign = HandSign.Undecided;

    public SkinType equippedSkin;

    public bool isPlayerA;
    public bool isReady;


    public string GetPlayerLetter()
    {
        if (RPSMatchMaking.Instance.localPlayer.isPlayerA)
        {
            return "playerA";
        }
        else
        {
            return "playerB";
        }
    }
}

public enum HandSign
{
    Undecided,
    Rock,
    Paper,
    Scissors
}

