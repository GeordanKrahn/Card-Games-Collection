using CardGames.Cards;
using System.Collections.Generic;
using UnityEngine;

namespace CardGames.Games.Cribbage
{
    public class TestCribbageHand : MonoBehaviour
    {
        [SerializeField] List<PlayingCard> hand;
        [SerializeField] PlayingCard cutCard;
        [SerializeField] HandOfCards handOfCardsBase;
        // Start is called before the first frame update
        void Start()
        {
            HandOfCards newHand = Instantiate(handOfCardsBase);
            newHand.Initialize(hand);

            CribbageHand cribbageHand = new CribbageHand(newHand, cutCard);
            Debug.Log($"Points: {cribbageHand.GetEvaluatedHand()}");
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}