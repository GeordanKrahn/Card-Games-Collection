using CardGames.Cards;
using UnityEngine;

namespace CardGames.Games.Cribbage
{
    public class CribbageGame : Game, IGame
    {
        [Header("Cribbage")]
        [Range(2, 3)] [SerializeField] int numberOfCribPlayers;
        [SerializeField] bool doubleCrib = false;
        CribbagePlayer player1;
        CribbagePlayer player2;
        CribbagePlayer player3;
        HandOfCards wildCard;
        HandOfCards crib1;
        HandOfCards crib2;
        const int MAX_SCORE = 121;
        CribbagePhase cribbagePhase;
        CribbagePlayerTurn cribbagePlayerTurn;
        CribbagePlayerCrib cribbagePlayerCrib;

        public void RunGame()
        {
            throw new System.NotImplementedException();
        }

        public void StartGame(Difficulty difficulty)
        {
            CreateDeck(1);
            deckOfCards.RemoveAllJokers();
            deckOfCards.Shuffle(3);
        }

        enum CribbagePhase
        {
            Shuffle, Deal, Discard, Cut, Play, CountHand, CountCrib
        }

        enum CribbagePlayerTurn
        {
            Player1, Player2, Player3
        }

        enum CribbagePlayerCrib
        {
            Player1, Player2, Player3
        }
    }
}