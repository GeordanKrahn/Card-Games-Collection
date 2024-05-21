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

        void Initialize(Suit suit, CardValue value)
        {
            // PlayingCard card = (PlayingCard)CreateInstance("PlayingCard");
            this.suit = suit;
            this.value = value;
        }

        public static PlayingCard CreateInstance(Suit suit, CardValue value)
        {
            PlayingCard card = CreateInstance<PlayingCard>();
            card.Initialize(suit, value);
            return card;
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

        public override bool Equals(object other)
        {
            return this == (PlayingCard)other;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return $"{GetCardValue()} of {GetSuit()}";
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