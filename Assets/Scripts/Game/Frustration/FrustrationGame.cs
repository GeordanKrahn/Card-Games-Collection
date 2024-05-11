using CardGames.Cards;
using UnityEngine;

namespace CardGames.Games.Frustration
{
    public class FrustrationGame : Game, IGame
    {
        [Header("Frustration")]
        [Range(2, 4)] [SerializeField] int numberOfFrustrationPlayers;
        [SerializeField] bool playFrustrationWithJokers = false;
        HandOfCards frustrationPlayer1Hand;
        HandOfCards frustrationPlayer2Hand;
        HandOfCards frustrationPlayer3Hand;
        HandOfCards frustrationPlayer4Hand;
        FrustationPhase frustationPhase;
        FrustrationPlayerTurn frustrationPlayerTurn;
        FrustrationPlayerDeal frustrationPlayerDeal;
        FrustationHand frustationHand;
        public void RunGame()
        {
            throw new System.NotImplementedException();
        }

        public void StartGame(Difficulty difficulty)
        {
            CreateDeck(1);
            if (!playFrustrationWithJokers)
            {
                deckOfCards.RemoveAllJokers();
            }
            deckOfCards.Shuffle(3);
        }

        enum FrustationPhase
        {
            Shuffle, Deal, Play
        }

        enum FrustrationPlayerTurn
        {
            Player1, Player2, Player3, Player4
        }

        enum FrustrationPlayerDeal
        {
            Player1, Player2, Player3, Player4
        }

        enum FrustationHand
        {
            Hand1, Hand2, Hand3
        }
    }
}