public class RPSPlayer
{
    public PlayerData playerData;

    public HandSign handSign = HandSign.Rock;

    public bool isReady;
    public bool isPlayerA;


    public RPSPlayer(PlayerData data)
    {
        playerData = data;
    }
}

public enum HandSign
{
    Rock,
    Paper,
    Scissors
}