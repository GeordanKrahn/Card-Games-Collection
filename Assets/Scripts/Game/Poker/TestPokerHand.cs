using CardGames.Cards;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPokerHand : MonoBehaviour
{
    [SerializeField] List<PlayingCard> hand;
    [SerializeField] List<PlayingCard> sortedHand;
    // Start is called before the first frame update
    void Start()
    {
        var descendingCards = from card in hand
                              orderby card.GetCardValue() descending
                              select card;
        foreach(var card in descendingCards)
        {
            sortedHand.Add(card);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
