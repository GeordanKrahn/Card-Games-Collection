using CardGames.Cards;
using CardGames.Pieces;
using UnityEngine;

namespace CardGames.Games.BlackJack
{
    public class BlackJackPlayer : MonoBehaviour
    {
        HandOfCards blackJackPlayerHand;
        HandOfCards blackJackPlayerSplitHand;
        int blackJackPlayerPoints;
        ChipCollection blackJackPlayerBet;
        ChipCollection blackJackPlayerChips;
        void Initialize(Transform handTransform, Transform splitHandTransform, int money)
        {

        }

        public ChipCollection GetBlackJackPlayerChips()
        {
            return blackJackPlayerChips;
        }

        public void SetBlackJackPlayerChips(ChipCollection collection)
        {
            blackJackPlayerChips = collection;
        }
    }
}