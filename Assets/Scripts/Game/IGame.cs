namespace CardGames.Games
{
    public interface IGame
    {
        public void StartGame(Difficulty difficulty);
        public void RunGame();
    }
}