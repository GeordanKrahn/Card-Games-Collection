using CardGames.Cards;
using UnityEngine;

namespace CardGames.Games.War
{
    public class WarGame : Game, IGame
    {
        [Header("War")]
        [SerializeField] bool playWarWithJokers = false;
        HandOfCards warPlayer1Hand;
        HandOfCards warPlayer2Hand;
        WarPhase warPhase;
        WarPlayerTurn warPlayerTurn;
        public void RunGame()
        {
            throw new System.NotImplementedException();
        }

        public void StartGame(Difficulty difficulty)
        {
            throw new System.NotImplementedException();
        }
        enum WarPhase
        {
            Shuffle, Deal, Battle, War
        }

        enum WarPlayerTurn
        {
            Player1, Player2
        }

    }
}