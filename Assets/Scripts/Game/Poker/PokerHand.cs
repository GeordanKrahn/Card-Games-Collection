using CardGames.Cards;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardGames.Games.Poker
{
    public class PokerHand
    {
        int numberOfCards;
        int handScore;
        int[] cardValues;

        PlayingCard[] hand = new PlayingCard[5];
        PlayingCard[] wildCards = new PlayingCard[4];

        public PokerHand(HandOfCards pokerHand, WildCardType wild)
        {
            numberOfCards = pokerHand.Hand.Count;
            if(numberOfCards < 2 || numberOfCards > 5)
            {
                throw new Exception($"Invalid Poker Hand Size - numberOfCards: {numberOfCards}");
            }
            else
            {
                for(int i = 0; i < numberOfCards; i++)
                {
                    hand[i] = pokerHand.Hand[i];
                }
            }
            if(wild != WildCardType.None)
            {
                SetWildCards(wild);
            }
            SetCardValues();
            EvaluateHand();
        }

        void SetCardValues()
        {
            var descendingCards = from card in hand
                                  orderby card.GetCardValue() descending
                                  select card;

            var sortedHand = descendingCards.ToArray();
            cardValues = new int[sortedHand.Length];
            for(int i = 0; i < sortedHand.Length; i++)
            {
                cardValues[i] = (int)sortedHand[i].GetCardValue();
            }
        }

        void EvaluateHand()
        {
            /********************************************************
             * Scoring Table
             * ******************************************************
             * No pair          = 0
             * Pair             = 1
             * 2 Pair           = 2
             * 3 of a Kind      = 3
             * Straight         = 4
             * Flush            = 5
             * Full House       = 6
             * 4 of a Kind      = 7
             * Straight Flush   = 8
             * 5 of a Kind      = 9
             *******************************************************/

            handScore = 0; // This value will be reset below, if there is no combination yielding points, it will not be changed.

            #region Duplicate Cards
            // Evaluate duplicate cards first

            // 5 of a kind
            #region 5 of a kind

            #endregion

            #region 4 of a kind

            // 4 of a kind
            if (handScore == 0)
            {
                // First find duplicate cards
                // If no duplicates are found, look for a wild card
                handScore = 7;
            }

            #endregion

            #region Full House
            // Full House
            if (handScore == 0)
            {
                // First find duplicate cards
                // If no duplicates are found, look for a wild card
                handScore = 6;
            }
            #endregion

            #region 3 of a kind
            // 3 of a kind
            if (handScore == 0)
            {
                // First find duplicate cards
                // If no duplicates are found, look for a wild card
                handScore = 3;
            }
            #endregion

            #region 2 Pair
            // 2 pair
            if (handScore == 0)
            {
                // First find duplicate cards
                // If no duplicates are found, look for a wild card
                handScore = 2;
            }
            #endregion

            #region Pair
            // Pair
            if (handScore == 0)
            {
                // First find duplicate cards
                // If no duplicates are found, look for a wild card
                handScore = 1;
            }
            #endregion
            #endregion

            #region Straights And Flushes
            // Evaluate Straight, Flush, and Straight Flush

            #region Straight Flush
            // Straight Flush
            if (handScore == 0)
            {
                if(IsFlush() && IsStraight())
                {
                    handScore = 8;
                }
            }
            #endregion

            #region Flush
            // Flush
            if (handScore == 0)
            {
                if(IsFlush())
                {
                    handScore = 5;
                }
            }
            #endregion

            #region Straight
            // Straight
            if (handScore == 0)
            {
                if(IsStraight())
                {
                    handScore = 4;
                }
            }
            #endregion
            #endregion
        }
        
        bool IsFlush()
        {
            Suit cardSuit = Suit.NONE;
            foreach(var card in hand)
            {
                if (IsWild(card)) continue;
                cardSuit = card.GetSuit();
                break;
            }
            for (int i = 0; i < hand.Length; i++)
            {
                if (IsWild(hand[i])) continue;
                if (hand[i].GetSuit() != cardSuit)
                {
                    return false;
                }
            }
            return true;
        }

        bool IsStraight()
        {
            // First sort the cards from highest to lowest
            var descendingCards = from card in hand
                                  orderby card.GetCardValue() descending
                                  select card;

            var sortedHand = descendingCards.ToArray();

            // Compare the card values, check if it is the next lowest card value
            int comparator = (int)sortedHand[0].GetCardValue();
            for(int i = 1; i < sortedHand.Length; i++)
            {
                if(comparator == (int)sortedHand[i].GetCardValue() - 1)
                {
                    comparator--;
                }
                else
                {
                    return false;
                }
            }
            return true;
        }

        int GetDuplicates(PlayingCard playingCard)
        {
            // TODO
            return 0;
        }

        void SetWildCards(WildCardType type)
        {
            switch(type)
            {
                case WildCardType.Aces:
                    GenerateWildCards(CardValue.Ace);
                    break;
                case WildCardType.Douces:
                    GenerateWildCards(CardValue.Two);
                    break;
                case WildCardType.Threes:
                    GenerateWildCards(CardValue.Three);
                    break;
                case WildCardType.Fours:
                    GenerateWildCards(CardValue.Four);
                    break;
                case WildCardType.Fives:
                    GenerateWildCards(CardValue.Five);
                    break;
                case WildCardType.Sixes:
                    GenerateWildCards(CardValue.Six);
                    break;
                case WildCardType.Sevens:
                    GenerateWildCards(CardValue.Seven);
                    break;
                case WildCardType.Eights:
                    GenerateWildCards(CardValue.Eight);
                    break;
                case WildCardType.Nines:
                    GenerateWildCards(CardValue.Nine);
                    break;
                case WildCardType.Tens:
                    GenerateWildCards(CardValue.Ten);
                    break;
                case WildCardType.Jacks:
                    GenerateWildCards(CardValue.Jack);
                    break;
                case WildCardType.OneEyedJacks:
                    GenerateOneEyedJacks();
                    break;
                case WildCardType.Queens:
                    GenerateWildCards(CardValue.Queen);
                    break;
                case WildCardType.Kings:
                    GenerateWildCards(CardValue.King);
                    break;
                case WildCardType.Jokers:
                    GenerateJokers();
                    break;
                default:
                    break;
            }
        }

        private void GenerateJokers()
        {
            wildCards = new PlayingCard[2];
            wildCards[0] = new PlayingCard(Suit.Diamonds, CardValue.Joker);
            wildCards[1] = new PlayingCard(Suit.Clubs, CardValue.Joker);
        }

        private void GenerateOneEyedJacks()
        {
            wildCards = new PlayingCard[2];
            wildCards[0] = new PlayingCard(Suit.Spades, CardValue.Jack);
            wildCards[1] = new PlayingCard(Suit.Hearts, CardValue.Jack);
        }

        private void GenerateWildCards(CardValue value)
        {
            for (int i = 0; i < 4; i++)
            {
                wildCards[i] = new PlayingCard((Suit)i, value);
            }
        }

        bool IsWild(PlayingCard playingCard)
        {
            if (wildCards.Length == 0) return false;
            else
            {
                foreach(var card in wildCards)
                {
                    if(card == playingCard)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public int GetEvaluatedHand()
        {
            return handScore;
        }

        public int[] GetCardValues()
        {
            return cardValues;
        }
    }

    public enum WildCardType
    {
        None, Aces, Douces, Threes, Fours, Fives, Sixes, Sevens, Eights, Nines, Tens, Jacks, OneEyedJacks, Queens, Kings, Jokers
    }
}