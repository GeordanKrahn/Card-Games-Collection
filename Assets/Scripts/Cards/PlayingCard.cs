using UnityEngine;

namespace CardGames.Cards
{
    [CreateAssetMenu(fileName = "Playing Cards", menuName = "New Playing Card", order = 0)]
    public class PlayingCard : ScriptableObject
    {
        [SerializeField] Suit suit;
        [SerializeField] CardValue value;
        [SerializeField] Sprite backSprite;
        [SerializeField] Sprite faceSprite;

        public PlayingCard(Suit suit, CardValue value)
        {
            this.suit = suit;
            this.value = value;
        }

        public static bool operator ==(PlayingCard a, PlayingCard b)
        {
            if(a.GetSuit() == b.GetSuit() && a.GetCardValue() == b.GetCardValue())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool operator !=(PlayingCard a, PlayingCard b)
        {
            if (a.GetSuit() != b.GetSuit() || a.GetCardValue() != b.GetCardValue())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public Suit GetSuit() => suit;
        public CardValue GetCardValue() => value;
        public Sprite GetBackSprite() => backSprite;
        public Sprite GetFaceSprite() => faceSprite;
    }

    public enum Suit
    {
        Hearts, Diamonds, Clubs, Spades, NONE
    }

    public enum CardValue
    {
        Joker, Ace, Two, Three, Four, Five, Six, Seven, Eight, Nine, Ten, Jack, Queen, King
    }
}