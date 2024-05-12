using CardGames.Cards;
using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace CardGames.Games.Poker
{
    public class PokerHand
    {
        int numberOfCards;
        int handScore;
        int[] cardValues;

        const int MAX_SUITS = 4;
        const int MAX_JOKERS_AND_ONE_EYED_JACKS = 2;

        PlayingCard[] hand = new PlayingCard[5];
        PlayingCard[] wildCards = new PlayingCard[4];

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
            handScore = 0; // This value will be reset below, if there is no combination yielding points, it will not be changed.

            #region Duplicate Cards
            // Evaluate duplicate cards first
            for(int i = 0; i < hand.Length; i++)
            {
                #region 5 of a kind and 4 of a kind
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
                #endregion

                #region Pair, 2 Pair, 3 of a kind and Full House
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
            if (handScore < 8)
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
            if (handScore < 4)
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
            for (int i = 0; i < hand.Length; i++)
            {
                if (IsWild(hand[i])) continue;
                for (int j = 0; j < hand.Length; j++)
                {
                    if (i == j) continue;
                    if (IsWild(hand[j])) continue;
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
            List<PlayingCard> sortedHand = GetSortedHand();

            // Find all wilds and place them at Top
            PlaceWildsAtTopOfList(sortedHand);

            // Correctly place wilds in straight
            ReorderWildsInStraight(sortedHand);
            return CheckIsStraight(sortedHand);
        }

        private bool CheckIsStraight(List<PlayingCard> sortedHand)
        {
            // Compare the card values, check if it is the next lowest card value
            int comparator = (int)sortedHand[0].GetCardValue() - 1;
            for (int i = 1; i < sortedHand.Count; i++)
            {
                int cardValue = (int)sortedHand[i].GetCardValue();
                if (IsWild(sortedHand[i]) || comparator == cardValue)
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

        private void ReorderWildsInStraight(List<PlayingCard> sortedHand)
        {
            // Compare elements at i and j
            for (int i = 0; i < sortedHand.Count; i++)
            {
                if (IsWild(sortedHand[i])) continue;
                // Find all wilds and place them at Top
                for (int j = 0; j < sortedHand.Count; j++)
                {
                    if (i == j) continue;
                    if (IsWild(sortedHand[j])) continue;
                    // Check if comparing sequential elements
                    if (i == j - 1)
                    {
                        // Compare card values
                        // Check if card values are sequential
                        // The difference between elements at i and j should be 1
                        if ((int)sortedHand[i].GetCardValue() - (int)sortedHand[j].GetCardValue() > 1)
                        {
                            // Find a wild card and place it a j in the sorted hand
                            for (int k = 0; k < sortedHand.Count; k++)
                            {
                                if (k < j && IsWild(sortedHand[k]))
                                {
                                    // Remove the wildcard from the list
                                    PlayingCard bufferCard = sortedHand[k];
                                    sortedHand.RemoveAt(k);

                                    // Place the wildcard at j-1
                                    sortedHand.Insert(j - 1, bufferCard);
                                }
                            }
                        }
                    }
                }
            }
        }

        private void PlaceWildsAtTopOfList(List<PlayingCard> sortedHand)
        {
            for (int i = 0; i < sortedHand.Count; i++)
            {
                if (IsWild(sortedHand[i]))
                {
                    // Remove card from list
                    PlayingCard bufferCard = sortedHand[i];
                    sortedHand.RemoveAt(i);

                    // Place card at top
                    sortedHand.Insert(0, bufferCard);
                }
            }
        }

        private List<PlayingCard> GetSortedHand()
        {
            var descendingCards = from card in hand
                                  orderby card.GetCardValue() descending
                                  select card;

            return descendingCards.ToList();
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
                default:
                case WildCardType.None:
                    break;
            }
        }

        private void GenerateJokers()
        {
            wildCards = new PlayingCard[MAX_JOKERS_AND_ONE_EYED_JACKS];
            wildCards[0] = PlayingCard.CreateInstance(Suit.Diamonds, CardValue.Joker);
            wildCards[1] = PlayingCard.CreateInstance(Suit.Clubs, CardValue.Joker);
        }

        private void GenerateOneEyedJacks()
        {
            wildCards = new PlayingCard[MAX_JOKERS_AND_ONE_EYED_JACKS];
            wildCards[0] = PlayingCard.CreateInstance(Suit.Spades, CardValue.Jack);
            wildCards[1] = PlayingCard.CreateInstance(Suit.Hearts, CardValue.Jack);
        }

        private void GenerateWildCards(CardValue value)
        {
            for (int i = 0; i < MAX_SUITS; i++)
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