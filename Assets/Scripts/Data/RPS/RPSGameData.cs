public class RPSGameData
{
    public string gameID;

    public string hostUserId;

    public RPSPlayer playerA;
    public RPSPlayer playerB;

    public string playerAJson;
    public string playerBJson;

    public bool playerAConnected;
    public bool playerBConnected;

    public GameResult gameResult;

    public RPSGameData(RPSPlayer playerData, string userID)
    {
        gameID = UnityEngine.Random.Range(0, int.MaxValue).ToString();

        playerA = playerData;
        playerAJson = UnityEngine.JsonUtility.ToJson(playerData.playerData);

        hostUserId = userID;

        CheckConnectedPlayers();
    }

    public void CheckConnectedPlayers()
    {
        playerAConnected = (playerAJson != null);
        playerBConnected = (playerBJson != null);
    }
}
