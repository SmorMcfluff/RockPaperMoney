public class RPSGameData
{
    private PlayerData playerA;
    private PlayerData playerB;

    private enum GameState
    {
        AWin,
        BWin,
        Tie,
        Ongoing
    }

    private GameState gameState;

    private bool playerAViewed;
    private bool playerBViewed;
}
