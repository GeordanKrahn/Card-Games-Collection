using UnityEngine;

namespace CardGames.Pieces
{
    [CreateAssetMenu(fileName = "Poker Chip", menuName = "New Poker Chip", order = 1)]
    public class Chip : ScriptableObject
    {
        [SerializeField] GameObject model;
        [SerializeField] int value;

        public int GetValue() => value;
        public GameObject GetModel() => model;
    }
}