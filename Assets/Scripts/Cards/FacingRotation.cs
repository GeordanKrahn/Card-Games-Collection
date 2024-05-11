using UnityEngine;

namespace CardGames.Cards
{
    public static class FacingRotation
    {
        public static readonly Quaternion faceDownRotation = Quaternion.Euler(270, 0, 0);
        public static readonly Quaternion faceUpRotation = Quaternion.Euler(90, 0, 0);
    }
}