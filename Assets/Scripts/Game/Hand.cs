using CardGames.Cards;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardGames.Games
{
    public abstract class Hand
    {
        [SerializeField] internal HandOfCards handOfCardsBase;
        internal PlayingCard[] hand;
        internal void InitializeHand()
        {
            for (int i = 0; i < hand.Length; i++)
            {
                hand[i] = PlayingCard.CreateInstance(hand[i].GetSuit(), hand[i].GetCardValue());
            }
        }

        internal abstract void EvaluateHand();
        public abstract int GetEvaluatedHand();
    }
}