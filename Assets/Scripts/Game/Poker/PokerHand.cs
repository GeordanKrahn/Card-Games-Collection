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
        bool noWilds = false;

        public PokerHand(HandOfCards pokerHand, WildCardType wild = WildCardType.None)
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
                InitializeHand();
            }
            if(wild != WildCardType.None)
            {
                SetWildCards(wild);
            }
            SetCardValues();
            EvaluateHand();
        }

        void InitializeHand()
        {
            for (int i = 0; i < hand.Length; i++)
            {
                hand[i] = PlayingCard.CreateInstance(hand[i].GetSuit(), hand[i].GetCardValue());
            }
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
            for(int i = 0; i < hand.Length; i++)
            {
                int currentHandScore = 0;
                if (handScore == 9) break;
                if (IsWild(hand[i])) continue;
                int count = GetDuplicates(hand[i], true);
                if(count == 5)
                {
                    // 5 of a kind
                    currentHandScore = 9;
                }
                else if (count == 4)
                {
                    // 4 of a kind
                    currentHandScore = 7;
                }

                #region 2 Pair and Full House
                else if (count == 3)
                {
                    // Evaluate for a full house
                    for(int j = i + 1; j < hand.Length; j++)
                    {
                        // Dont evaluate the same card value
                        if (hand[i].GetCardValue() == hand[j].GetCardValue()) continue;
                        else
                        {
                            int secondCount = GetDuplicates(hand[j], false);
                            if(secondCount == 2)
                            {
                                // full house
                                currentHandScore = 6;
                                break;
                            }
                            else
                            {
                                // 3 of a kind
                                currentHandScore = 3;
                                break;
                            }
                        }
                    }
                }
                else if (count == 2)
                {
                    // Evaluate for a full house of a 2 pair
                    for (int j = i + 1; j < hand.Length; j++)
                    {
                        // Dont evaluate the same card value
                        if (hand[i].GetCardValue() == hand[j].GetCardValue()) continue;
                        else
                        {
                            int secondCount = GetDuplicates(hand[j], false);
                            if (secondCount == 3)
                            {
                                // full house
                                currentHandScore = 6;
                                break;
                            }
                            else if (secondCount == 2)
                            {
                                // 2 pair
                                currentHandScore = 2;
                                break;
                            }
                            else
                            {
                                // pair
                                currentHandScore = 1;
                                break;
                            }
                        }
                    }
                }
                #endregion
                if (currentHandScore >= handScore)
                {
                    handScore = currentHandScore;
                }
            }
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
            Debug.Log($"HandScore: {handScore}");
        }
        
        bool IsFlush()
        {
            // TODO: Make wild cards valid
            for (int i = 0; i < hand.Length; i++)
            {
                for (int j = 0; j < hand.Length; j++)
                {
                    if (i == j) continue;
                    if (hand[i].GetSuit() != hand[j].GetSuit())
                    {
                        return false;
                    }
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

            // TODO: Make wild cards valid

            int comparator = (int)sortedHand[0].GetCardValue() - 1;
            for(int i = 1; i < sortedHand.Length; i++)
            {
                int cardValue = (int)sortedHand[i].GetCardValue();
                if(comparator == cardValue)
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

        int GetDuplicates(PlayingCard playingCard, bool useWilds)
        {
            int count = 1; // Start count at one, if no duplicates, we only have single cards
            foreach(var card in hand)
            {
                if (card == playingCard) continue;
                if(useWilds)
                {
                    if (IsWild(card) || card.GetCardValue() == playingCard.GetCardValue())
                    {
                        count++;
                    }
                }
                else
                {
                    if (card.GetCardValue() == playingCard.GetCardValue())
                    {
                        count++;
                    }
                }
            }
            return count;
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
                case WildCardType.None:
                    noWilds = true;
                    break;
            }
        }

        private void GenerateJokers()
        {
            wildCards = new PlayingCard[2];
            wildCards[0] = PlayingCard.CreateInstance(Suit.Diamonds, CardValue.Joker);
            wildCards[1] = PlayingCard.CreateInstance(Suit.Clubs, CardValue.Joker);
        }

        private void GenerateOneEyedJacks()
        {
            wildCards = new PlayingCard[2];
            wildCards[0] = PlayingCard.CreateInstance(Suit.Spades, CardValue.Jack);
            wildCards[1] = PlayingCard.CreateInstance(Suit.Hearts, CardValue.Jack);
        }

        private void GenerateWildCards(CardValue value)
        {
            for (int i = 0; i < 4; i++)
            {
                wildCards[i] = PlayingCard.CreateInstance((Suit)i, value);
            }
        }

        bool IsWild(PlayingCard playingCard)
        {
            try
            {
                playingCard = PlayingCard.CreateInstance(playingCard.GetSuit(), playingCard.GetCardValue());
                if (wildCards.Length == 0) return false;
                else
                {
                    for (int i = 0; i < wildCards.Length; i++)
                    {
                        wildCards[i] = PlayingCard.CreateInstance(wildCards[i].GetSuit(), wildCards[i].GetCardValue());
                        Debug.Log($"WildCard: {wildCards[i].GetCardValue()} of {wildCards[i].GetSuit()}");
                        if (wildCards[i].GetSuit() == playingCard.GetSuit() && wildCards[i].GetCardValue() == playingCard.GetCardValue())
                        {
                            return true;
                        }
                    }
                    return false;
                }
            }
            catch (Exception e)
            {
                Debug.LogWarning(e);
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