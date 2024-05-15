using CardGames.Cards;
using System;
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
            //points += GetAllPairs();
            //points += GetAllRuns();
            //points += GetFlush();
            //points += GetHisNobs();
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
                for (int j = 0; j < cards.Count; j++)
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
            throw new NotImplementedException();
        }

        private int GetAllRuns()
        {
            throw new NotImplementedException();
        }

        private int GetFlush()
        {
            throw new NotImplementedException();
        }

        private int GetHisNobs()
        {
            throw new NotImplementedException();
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