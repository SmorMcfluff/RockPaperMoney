public class RPSGameData
{
    public string gameID;

    public RPSPlayer playerA;
    public RPSPlayer playerB;

    public string playerAJson;
    public string playerBJson;

    public bool playerAConnected;
    public bool playerBConnected;

    public GameResult gameResult;

    public RPSGameData(RPSPlayer playerData)
    {
        gameID = UnityEngine.Random.Range(0, int.MaxValue).ToString();

        playerA = playerData;
        playerAJson = UnityEngine.JsonUtility.ToJson(playerData.playerData);

        CheckConnectedPlayers();
    }

    public void CheckConnectedPlayers()
    {
        playerAConnected = (playerAJson != null);
        playerBConnected = (playerBJson != null);
    }
}
