using UnityEngine;

namespace CardGames.Games
{
    public class GameManager : MonoBehaviour
    {
        [Header("Game")]
        [SerializeField] Game game;
        [SerializeField] Difficulty difficulty;

        // Start is called before the first frame update
        void Start()
        {
            game.GetComponent<IGame>().StartGame(difficulty);
        }

        // Update is called once per frame
        void Update()
        {
            game.GetComponent<IGame>().RunGame();
        }
    }

    #region GameConfiguration
    public enum Difficulty
    {
        Easy, Normal, Hard, VeryHard
    }
    #endregion
}