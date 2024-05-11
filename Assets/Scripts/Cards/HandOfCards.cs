using System.Collections.Generic;
using UnityEngine;

namespace CardGames.Cards
{
    public class HandOfCards : MonoBehaviour
    {
        List<PlayingCard> hand;
        public List<PlayingCard> Hand { get => hand; }
        public void AddCard(PlayingCard card)
        {
            hand.Add(card);
        }

        public void SwapCard(ref HandOfCards swapHand)
        {
            if (hand.Count > 0)
            {
                var card = hand[hand.Count - 1];
                hand.Remove(card);
                swapHand.AddCard(card);
                RemoveCard(card);
            }
        }

        public void SwapCard(ref DiscardPile pile)
        {
            if (hand.Count > 0)
            {
                var card = hand[hand.Count - 1];
                hand.Remove(card);
                pile.AddCard(card);
                RemoveCard(card);
            }
        }

        // This May Break
        // TODO: 
        private void RemoveCard(PlayingCard card)
        {
            var children = new List<PlayingCardInstance>(GetComponentsInChildren<PlayingCardInstance>());
            foreach (var instance in children)
            {
                if (instance.GetPlayingCard() == card)
                {
                    Destroy(children[children.IndexOf(instance)].gameObject);
                }
            }
        }
    }
}