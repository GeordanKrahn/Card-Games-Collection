using UnityEngine;

namespace CardGames.Cards
{
    public class PlayingCardInstance : MonoBehaviour
    {
        [SerializeField] PlayingCard cardInstance;
        [SerializeField] SpriteRenderer frontSide, backSide;
        public void SetCard(PlayingCard newCard)
        {
            cardInstance = newCard;
            frontSide.sprite = cardInstance.GetFaceSprite();
            backSide.sprite = cardInstance.GetBackSprite();
        }

        public PlayingCard GetPlayingCard() => cardInstance;
    }
}