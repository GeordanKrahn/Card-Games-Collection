using CardGames.Cards;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace CardGames.Games.Poker
{
    public class TestPokerHand : MonoBehaviour
    {
        [SerializeField] List<PlayingCard> hand;
        [SerializeField] List<PlayingCard> sortedHand;
        [SerializeField] WildCardType wild;
        [SerializeField] HandOfCards handOfCardsBase;
        PlayingCard[] wildCards = new PlayingCard[4];
        // Start is called before the first frame update
        void Start()
        {
            // Generate Wild Cards
            if (wild != WildCardType.None)
            {
                SetWildCards(wild);
            }

            // First sort the cards from highest to lowest
            sortedHand = GetSortedHand();

            // Find all wilds and place them at Top
            PlaceWildsAtTopOfList(sortedHand);

            // Correctly place wilds in straight
            ReorderWildsInStraight(sortedHand);

            bool isStraight = CheckIsStraight(sortedHand);
            if(isStraight)
            {
                Debug.Log("Is Straight Hand");
            }

            for (int i = 0; i < hand.Count; i++)
            {
                hand[i] = PlayingCard.CreateInstance(hand[i].GetSuit(), hand[i].GetCardValue());
            }

            HandOfCards newHand = Instantiate(handOfCardsBase);
            newHand.Initialize(hand);

            PokerHand pokerHand = new PokerHand(newHand, wild);
            switch(pokerHand.GetEvaluatedHand())
            {
                case 0:
                    Debug.Log("No Pair");
                    break;
                case 1:
                    Debug.Log("Pair");
                    break;
                case 2:
                    Debug.Log("2 Pair");
                    break;
                case 3:
                    Debug.Log("3 of a Kind");
                    break;
                case 4:
                    Debug.Log("Straight");
                    break;
                case 5:
                    Debug.Log("Flush");
                    break;
                case 6:
                    Debug.Log("Full House");
                    break;
                case 7:
                    Debug.Log("4 of a Kind");
                    break;
                case 8:
                    Debug.Log("Straight Flush");
                    break;
                case 9:
                    Debug.Log("5 of a Kind");
                    break;
            }
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
                // Skip WildCards
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
        void SetWildCards(WildCardType type)
        {
            switch (type)
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
    }
}