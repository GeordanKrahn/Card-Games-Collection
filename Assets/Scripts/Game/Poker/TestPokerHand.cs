using CardGames.Cards;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardGames.Games.Poker
{
    public class TestPokerHand : MonoBehaviour
    {
        [SerializeField] List<PlayingCard> hand;
        [SerializeField] List<PlayingCard> sortedHand;
        [SerializeField] WildCardType wild;
        [SerializeField] HandOfCards handOfCardsBase;
        // Start is called before the first frame update
        void Start()
        {
            var descendingCards = from card in hand
                                  orderby card.GetCardValue() descending
                                  select card;
            foreach (var card in descendingCards)
            {
                sortedHand.Add(card);
            }

            for(int i = 0; i < hand.Count; i++)
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

        // Update is called once per frame
        void Update()
        {

        }
    }
}