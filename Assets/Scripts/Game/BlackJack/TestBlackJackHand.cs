using CardGames.Cards;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardGames.Games.BlackJack
{
    public class TestBlackJackHand : MonoBehaviour
    {
        [SerializeField] List<PlayingCard> hand;
        [SerializeField] HandOfCards handOfCardsBase;
        // Start is called before the first frame update
        void Start()
        {
            HandOfCards newHand = Instantiate(handOfCardsBase);
            newHand.Initialize(hand);

            BlackJackHand blackJackHand = new BlackJackHand(newHand);
            int points = blackJackHand.GetEvaluatedHand();
            if (blackJackHand.IsBlackJack())
            {
                Debug.Log("BlackJack");
            }
            else if(blackJackHand.IsBust())
            {
                Debug.Log($"Bust - {points}");
            }
            else
            {
                Debug.Log($"Points - {points}");
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}