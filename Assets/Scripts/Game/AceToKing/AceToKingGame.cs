using CardGames.Cards;
using UnityEngine;

namespace CardGames.Games.AceToKing
{
    public class AceToKingGame : Game, IGame
    {
        [Header("AceToKing")]
        [Range(4, 8)] [SerializeField] int numberOfAceToKingPlayers;
        HandOfCards aceToKingPlayer1Hand;
        HandOfCards aceToKingPlayer2Hand;
        HandOfCards aceToKingPlayer3Hand;
        HandOfCards aceToKingPlayer4Hand;
        HandOfCards aceToKingPlayer5Hand;
        HandOfCards aceToKingPlayer6Hand;
        HandOfCards aceToKingPlayer7Hand;
        HandOfCards aceToKingPlayer8Hand;
        AceToKingPhase aceToKingPhase;
        AceToKingPlayerTurn aceToKingPlayerTurn;
        AceToKingRound aceToKingRound;
        public void RunGame()
        {
            throw new System.NotImplementedException();
        }

        public void StartGame(Difficulty difficulty)
        {
            throw new System.NotImplementedException();
        }
        enum AceToKingPhase
        {
            Shuffle, Deal, Play
        }

        enum AceToKingPlayerTurn
        {
            Player1, Player2, Player3, Player4, Player5, Player6, Player7, Player8
        }

        enum AceToKingRound
        {
            Ace, Two, Three, Four, Five, Six, Seven, Eight, Nine, Ten, Jack, Queen, King
        }

    }
}