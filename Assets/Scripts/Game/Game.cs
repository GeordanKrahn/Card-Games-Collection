using CardGames.Cards;
using UnityEngine;

public abstract class Game : MonoBehaviour
{
    [SerializeField] DiscardPile pile;
    [SerializeField] DeckOfCards deckBase;
    internal DeckOfCards deckOfCards;
    internal void CreateDeck(int numberOfDecks)
    {
        deckOfCards = Instantiate(deckBase, transform);
        if (numberOfDecks > 1)
        {
            deckOfCards.AddDeck(numberOfDecks - 1);
        }
    }

    public void FlipCard()
    {
        deckOfCards.DealTopCard(ref pile);
    }

    public void Shuffle(int numberOfShuffles)
    {
        deckOfCards.Shuffle(numberOfShuffles);
    }

    public void RestoreToDeck()
    {
        pile.RestoreToDeck(deckOfCards);
    }
}