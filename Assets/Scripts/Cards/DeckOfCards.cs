using System.Collections.Generic;
using UnityEngine;

namespace CardGames.Cards
{
    public class DeckOfCards : MonoBehaviour
    {
        [SerializeField] List<PlayingCard> deck;
        [SerializeField] PlayingCardInstance cardBase;

        public List<PlayingCard> Deck { get => deck; }

        public void Remove2Jokers()
        {
            var tempList = new List<PlayingCard>(deck);
            foreach (var card in deck)
            {
                if ((card.GetCardValue() == CardValue.Joker && card.GetSuit() == Suit.Hearts) ||
                    (card.GetCardValue() == CardValue.Joker && card.GetSuit() == Suit.Spades))
                {
                    tempList.Remove(card);
                }
            }
            deck = new List<PlayingCard>(tempList);
        }

        public void RemoveAllJokers()
        {
            var tempList = new List<PlayingCard>(deck);
            foreach (var card in deck)
            {
                if (card.GetCardValue() == CardValue.Joker)
                {
                    tempList.Remove(card);
                }
            }
            deck = new List<PlayingCard>(tempList);
        }

        public void Shuffle(int numberOfShuffles)
        {
            RemoveChildren();

            for (int i = 0; i < numberOfShuffles; i++)
            {
                var tempList = new List<PlayingCard>(deck);
                foreach (var card in deck)
                {
                    tempList.Remove(card);
                    // Random.InitState(DateTime.Now.Millisecond + i);
                    int index = Random.Range(0, tempList.Count - 1);
                    tempList.Insert(index, card);
                }
                deck = new List<PlayingCard>(tempList);
            }

            SpawnCards();
        }

        public void AddCard(PlayingCard card)
        {
            deck.Add(card);
            float depth = cardBase.GetComponent<BoxCollider>().size.z;
            int cardCount = deck.Count;
            SpawnCard(Vector3.zero, depth, cardCount, card);
        }

        public void DealTopCard(ref HandOfCards hand)
        {
            if (deck.Count > 0)
            {
                var card = deck[deck.Count - 1];
                deck.Remove(card);
                hand.AddCard(card);
                RemoveCardAtIndex(GetComponentsInChildren<PlayingCardInstance>(), deck.Count);
            }
        }

        public void DealTopCard(ref DiscardPile pile)
        {
            if (deck.Count > 0)
            {
                var card = deck[deck.Count - 1];
                deck.RemoveAt(deck.Count - 1);
                pile.AddCard(card);
                RemoveCardAtIndex(GetComponentsInChildren<PlayingCardInstance>(), deck.Count);
            }
        }

        void RemoveChildren()
        {
            var children = GetComponentsInChildren<PlayingCardInstance>();
            for (int i = 0; i < children.Length; i++)
            {
                RemoveCardAtIndex(children, i);
            }
        }

        private static void RemoveCardAtIndex(PlayingCardInstance[] children, int i)
        {
            Destroy(children[i].gameObject);
        }

        void SpawnCards()
        {
            var position = Vector3.zero;
            float depth = cardBase.GetComponent<BoxCollider>().size.z;
            int cardCount = 1;
            foreach (var card in deck)
            {
                SpawnCard(position, depth, cardCount, card);
                cardCount++;
            }
        }

        private void SpawnCard(Vector3 position, float depth, int numberOfCards, PlayingCard card)
        {
            var newCard = Instantiate(cardBase, transform);
            newCard.SetCard(card);
            newCard.transform.rotation = FacingRotation.faceDownRotation;
            newCard.transform.localPosition = new Vector3(position.x, depth * numberOfCards, position.z);
        }

        public void AddDeck(int numberOfDecks)
        {
            List<PlayingCard> newDeck = new List<PlayingCard>(deck);
            for (int i = 0; i < numberOfDecks; i++)
            {
                foreach (var card in newDeck)
                {
                    deck.Add(card);
                }
            }
        }
    }
}