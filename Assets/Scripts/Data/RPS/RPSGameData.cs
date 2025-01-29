public class RPSGameData
{
    public string gameID;

    public RPSPlayer playerA;
    public RPSPlayer playerB;

    public string playerAJson;
    public string playerBJson;

    public bool playerAConnected;
    public bool playerBConnected;

    public bool playerAReady;
    public bool playerBReady;

    public enum GameState
    {
        Undecided,
        AWin,
        BWin,
        Tie,
    }

    public GameState gameState = GameState.Undecided;

    public RPSGameData(RPSPlayer data)
    {
        gameID = UnityEngine.Random.Range(0, int.MaxValue).ToString();

        playerA = data;
        playerAJson = UnityEngine.JsonUtility.ToJson(data.playerData);
        CheckConnectedPlayers();

    }

    public void CheckConnectedPlayers()
    {
        playerAConnected = playerAJson != null;
        playerBConnected = playerBJson != null;
    }
}
