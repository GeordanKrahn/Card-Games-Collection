using CardGames.Cards;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardGames.Games.Cribbage
{
    public class CribbageHand : Hand
    {
        PlayingCard cutCard;
        bool isCrib;
        public CribbageHand(HandOfCards cribbageHand, PlayingCard cutCard, bool isCrib = false)
        {
            hand = new PlayingCard[cribbageHand.Hand.Count];
            this.cutCard = cutCard;
            this.isCrib = isCrib;
            int numberOfCards = cribbageHand.Hand.Count;
            if (numberOfCards != 4)
            {
                throw new Exception($"Invalid Cribbage Hand Size - numberOfCards: {numberOfCards}");
            }
            else
            {
                for (int i = 0; i < numberOfCards; i++)
                {
                    hand[i] = cribbageHand.Hand[i];
                }
                InitializeHand();
            }
            EvaluateHand();
        }
        internal override void EvaluateHand()
        {
            points += GetAllFifteens();
            points += GetAllPairs();
            points += GetAllRuns();
            points += GetFlush();
            points += GetHisNobs();
        }

        private int GetAllFifteens()
        {
            int total = 0;
            List<PlayingCard> cards = new List<PlayingCard>(hand);
            cards.Add(cutCard);

            // Start by counting all 5 cards
            if (GetPoints(cards[0]) +
               GetPoints(cards[1]) +
               GetPoints(cards[2]) +
               GetPoints(cards[3]) +
               GetPoints(cards[4]) == 15)
            {
                total += 2;
            }

            // Count 4 cards
            for (int i = 0; i < cards.Count - 3; i++)
            {
                for (int j = 1; j < cards.Count - 2; j++)
                {
                    if (i >= j) continue;
                    for (int k = 2; k < cards.Count - 1; k++)
                    {
                        if (i >= k || j >= k) continue;
                        for (int l = 3; l < cards.Count; l++)
                        {
                            if (i >= l || j >= l || k >= l) continue;
                            if (GetPoints(cards[i]) +
                                GetPoints(cards[j]) +
                                GetPoints(cards[k]) +
                                GetPoints(cards[l]) == 15)
                            {
                                total += 2;
                            }
                        }
                    }
                }
            }
            // count 3 cards
            for (int i = 0; i < cards.Count - 2; i++)
            {
                for (int j = 1; j < cards.Count - 1; j++)
                {
                    if (i >= j) continue;
                    for (int k = 2; k < cards.Count; k++)
                    {
                        if (i >= k || j >= k) continue;
                        if (GetPoints(cards[i]) +
                            GetPoints(cards[j]) +
                            GetPoints(cards[k]) == 15)
                        {
                            total += 2;
                        }
                    }
                }
            }
            // count 2 cards
            for (int i = 0; i < cards.Count - 1; i++)
            {
                for (int j = 1; j < cards.Count; j++)
                {
                    if (i >= j) continue;
                    if (GetPoints(cards[i]) +
                        GetPoints(cards[j]) == 15)
                    {
                        total += 2;
                    }
                }
            }
            return total;
        }

        private int GetAllPairs()
        {
            int total = 0;
            List<PlayingCard> cards = new List<PlayingCard>(hand);
            cards.Add(cutCard);
            for (int i = 0; i < cards.Count - 1; i++)
            {
                for (int j = 0; j < cards.Count; j++)
                {
                    if (i >= j) continue;
                    if (cards[i].GetSuit() == cards[j].GetSuit())
                    {
                        total += 2;
                    }
                }
            }
            return total;
        }

        private int GetAllRuns()
        {
            int total = 0;
            List<PlayingCard> handWithCutCard = new List<PlayingCard>(hand);
            handWithCutCard.Add(cutCard);
            
            // sort hand in descending order
            var descendingCards = from card in handWithCutCard
                                  orderby card.GetCardValue() descending
                                  select card;
            List<PlayingCard> sortedHand = descendingCards.ToList();

            // Determine run of 5
                // Compare the card values, check if it is the next lowest card value
            int comparator = (int)sortedHand[0].GetCardValue() - 1;
            int count = 1;
            bool hasRunOf5 = false;
            for (int i = 1; i < sortedHand.Count; i++)
            {
                int cardValue = (int)sortedHand[i].GetCardValue();
                if (comparator == cardValue)
                {
                    comparator--;
                    count++;
                }
                else
                {
                    break;
                }
                if(count > 5)
                {
                    total = 5;

                    hasRunOf5 = true;
                }
            }

            if (!hasRunOf5)
            {
                bool hasRunOf4 = false;
                // Determine runs of 4
                // single run of 4
                comparator = (int)sortedHand[0].GetCardValue() - 1;
                count = 1;
                for (int i = 1; i < sortedHand.Count; i++)
                {
                    int cardValue = (int)sortedHand[i].GetCardValue();
                    if (comparator == cardValue)
                    {
                        comparator--;
                        count++;
                    }
                    else
                    {
                        if(count == 1)
                        {
                            comparator = (int)sortedHand[1].GetCardValue() - 1;
                        }
                    }
                    if (count > 4)
                    {
                        total = 4;
                        hasRunOf4 = true;
                    }
                }

                // double run of 4
                // find duplicate cards if any
                int duplicateIndex1 = 0;
                int duplicateIndex2 = 1;
                bool hasDuplicates = false;
                for (int i = 0; i < sortedHand.Count; i++)
                {
                    if (hasDuplicates) break;
                    for (int j = 1; j < sortedHand.Count; j++)
                    {
                        if (hasDuplicates) break;
                        if (j == i + 1)
                        {
                            if (sortedHand[i].GetCardValue() == sortedHand[j].GetCardValue())
                            {
                                hasDuplicates = true;
                            }
                        }
                        else continue;
                    }
                }

                if(hasDuplicates)
                {
                    if(hasRunOf4)
                    {
                        total = 0; // prevent counting of extra points for some cases where the single run is already counted.
                    }

                    // Create 2 different 4 card hands with the different duplicate cards
                    List<PlayingCard> potentialRun1 = new List<PlayingCard>(sortedHand);
                    potentialRun1.Remove(sortedHand[duplicateIndex1]);

                    comparator = (int)potentialRun1[0].GetCardValue() - 1;
                    count = 1;
                    for (int i = 1; i < potentialRun1.Count; i++)
                    {
                        int cardValue = (int)potentialRun1[i].GetCardValue();
                        if (comparator == cardValue)
                        {
                            comparator--;
                            count++;
                        }
                        else
                        {
                            break;
                        }
                        if (count > 4)
                        {
                            total += 4;
                            hasRunOf4 = true;
                        }
                    }

                    List<PlayingCard> potentialRun2 = new List<PlayingCard>(sortedHand);
                    potentialRun2.Remove(sortedHand[duplicateIndex2]);

                    comparator = (int)potentialRun2[0].GetCardValue() - 1;
                    count = 1;
                    for (int i = 1; i < potentialRun2.Count; i++)
                    {
                        int cardValue = (int)potentialRun2[i].GetCardValue();
                        if (comparator == cardValue)
                        {
                            comparator--;
                            count++;
                        }
                        else
                        {
                            break;
                        }
                        if (count > 4)
                        {
                            total += 4;
                            hasRunOf4 = true;
                        }
                    }
                }

                if (!hasRunOf4)
                {
                    // Determine runs of 3

                    // single run of 3

                    // double run of 3

                    // triple run of 3

                    // quadruple (double double) run of 3
                }
            }

            return total;
        }

        private int GetFlush()
        {
            bool isFlush = true;
            int total = 0;
            Suit suit = hand[0].GetSuit();
            for (int i = 1; i < hand.Length; i++)
            {
                if(hand[i].GetSuit() != suit)
                {
                    isFlush = false;
                }
            }
            if(isFlush)
            {
                total = 4;
            }
            if(isCrib && cutCard.GetSuit() != suit)
            {
                total = 0;
            }
            else if (isCrib && cutCard.GetSuit() == suit)
            {
                total = 5;
            }
            if(isCrib && !isCrib && cutCard.GetSuit() == suit)
            {
                total = 5;
            }
            return total;
        }

        private int GetHisNobs()
        {
            int total = 0;
            foreach(var card in hand)
            {
                if(card.GetCardValue() == CardValue.Jack)
                {
                    if(card.GetSuit() == cutCard.GetSuit())
                    {
                        total = 1;
                    }
                }
            }
            return total;
        }

        private int GetPoints(PlayingCard playingCard)
        {
            switch (playingCard.GetCardValue())
            {
                case CardValue.Ace:
                    return 1;
                case CardValue.Two:
                    return 2;
                case CardValue.Three:
                    return 3;
                case CardValue.Four:
                    return 4;
                case CardValue.Five:
                    return 5;
                case CardValue.Six:
                    return 6;
                case CardValue.Seven:
                    return 7;
                case CardValue.Eight:
                    return 8;
                case CardValue.Nine:
                    return 9;
                case CardValue.Ten:
                case CardValue.Jack:
                case CardValue.Queen:
                case CardValue.King:
                    return 10;
                default:
                    return 0;
            }
        }
    }
}