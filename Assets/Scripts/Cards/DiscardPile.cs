using System.Collections.Generic;
using UnityEngine;

namespace CardGames.Cards
{
    public class DiscardPile : MonoBehaviour
    {
        [SerializeField] List<PlayingCard> discardPile;
        [SerializeField] PlayingCardInstance cardBase;

        public List<PlayingCard> Pile { get => discardPile; }

        public void AddCard(PlayingCard card)
        {
            discardPile.Add(card);
            var depth = cardBase.GetComponent<BoxCollider>().size.z;
            SpawnCard(Vector3.zero, depth, discardPile.Count - 1, card);
        }

        public void Discard(PlayingCard card)
        {
            discardPile.Remove(card);
            var children = GetComponentsInChildren<PlayingCardInstance>();
            foreach (var cardInstance in children)
            {
                if (cardInstance.GetPlayingCard() == card)
                {
                    Destroy(cardInstance.gameObject);
                }
            }
        }

        private void SpawnCard(Vector3 position, float depth, int numberOfCards, PlayingCard card)
        {
            var newCard = Instantiate(cardBase, transform);
            newCard.SetCard(card);
            newCard.transform.rotation = FacingRotation.faceUpRotation;
            newCard.transform.localPosition = new Vector3(position.x, depth * numberOfCards, position.z);
        }

        public void RestoreToDeck(DeckOfCards deck)
        {
            var tempList = new List<PlayingCard>(discardPile);
            foreach (var card in discardPile)
            {
                deck.AddCard(card);
                tempList.Remove(card);
            }
            discardPile = new List<PlayingCard>(tempList);

            var children = GetComponentsInChildren<PlayingCardInstance>();
            foreach (var cardInstance in children)
            {
                Destroy(cardInstance.gameObject);
            }
        }
    }
}