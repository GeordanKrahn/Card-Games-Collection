using CardGames.Cards;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardGames.Games.BlackJack
{
    public class BlackJackHand : Hand
    {
        const int BLACK_JACK = 21;
        const int ACES_LOW = 10;
        public BlackJackHand(HandOfCards blackJackHand)
        {
            hand = new PlayingCard[blackJackHand.Hand.Count];
            int numberOfCards = blackJackHand.Hand.Count;
            if (numberOfCards < 2)
            {
                throw new Exception($"Invalid BlackJAck Hand Size - numberOfCards: {numberOfCards}");
            }
            else
            {
                for (int i = 0; i < numberOfCards; i++)
                {
                    hand[i] = blackJackHand.Hand[i];
                }
                InitializeHand();
            }
            EvaluateHand();
        }

        public bool IsBlackJack()
        {
            return points == BLACK_JACK;
        }

        public bool IsBust()
        {
            return points > BLACK_JACK;
        }

        internal override void EvaluateHand()
        {
            foreach(var card in hand)
            {
                points += GetPoints(card);
            }

            if(points > BLACK_JACK)
            {
                foreach(var card in hand)
                {
                    if(card.GetCardValue() == CardValue.Ace)
                    {
                        points -= ACES_LOW;
                        if (points < BLACK_JACK) break;
                    }
                }
            }
        }

        private int GetPoints(PlayingCard card)
        {
            int value = 0;
            switch(card.GetCardValue())
            {
                case CardValue.Ace:
                    value = 11;
                    break;
                case CardValue.Two:
                    value = 2;
                    break;
                case CardValue.Three:
                    value = 3;
                    break;
                case CardValue.Four:
                    value = 4;
                    break;
                case CardValue.Five:
                    value = 5;
                    break;
                case CardValue.Six:
                    value = 6;
                    break;
                case CardValue.Seven:
                    value = 7;
                    break;
                case CardValue.Eight:
                    value = 8;
                    break;
                case CardValue.Nine:
                    value = 9;
                    break;
                case CardValue.Ten:
                case CardValue.Jack:
                case CardValue.Queen:
                case CardValue.King:
                    value = 10;
                    break;
            }
            return value;
        }
    }
}