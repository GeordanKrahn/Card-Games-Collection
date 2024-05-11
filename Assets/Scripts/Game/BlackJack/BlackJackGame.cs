using CardGames.Pieces;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CardGames.Games.BlackJack
{
    public class BlackJackGame : Game, IGame
    {
        [Header("BlackJack")]
        [Range(1, 3)] [SerializeField] int numberOfBlackJackPlayers;
        [SerializeField] ChipCollection chipCollection;
        [SerializeField] GameObject blackJackPanel;
        [SerializeField] GameObject blackJackBetting;
        [SerializeField] Button placebet;
        [SerializeField] TMP_InputField betAmount;
        [SerializeField] GameObject blackJackDealing;
        [SerializeField] GameObject blackJackPlaying;
        [SerializeField] GameObject blackJackDealer;
        [SerializeField] GameObject blackJackSettlement;
        [SerializeField] GameObject blackJackShuffle;
        [SerializeField] BlackJackPlayer blackJackPlayerBase;

        BlackJackDealer dealer;
        BlackJackPlayer blackJackPlayer1;
        BlackJackPlayer blackJackPlayer2;
        BlackJackPlayer blackJackPlayer3;
        BlackJackPhase blackJackPhase;
        BlackJackPlayerTurn blackJackPlayerTurn;

        char[] prohibitedCharacters = "abcdefghijklmnopqrstuvwxyz`~!@#$%^&*()-_=+[{]}\\|;:'\",<.>/?".ToCharArray();
        public void RunGame()
        {
            switch (blackJackPhase)
            {
                case BlackJackPhase.Betting:
                    BlackJackBetting();
                    break;
                case BlackJackPhase.Dealing:
                    BlackJackDealing();
                    break;
                case BlackJackPhase.PlayersPlay:
                    BlackJackPlayersPlay();
                    break;
                case BlackJackPhase.DealersPlay:
                    BlackJackDealersPlay();
                    break;
                case BlackJackPhase.Settlement:
                    BlackJackSettlement();
                    break;
                case BlackJackPhase.Shuffle:
                    BlackJackShuffle();
                    break;
            }
        }

        public void StartGame(Difficulty difficulty)
        {
            CreateBlackJackDeck(difficulty);
            deckOfCards.RemoveAllJokers();
            deckOfCards.Shuffle(100);

            blackJackPlayer1 = Instantiate(blackJackPlayerBase, transform);

            blackJackPlayer1.SetBlackJackPlayerChips(
                Instantiate(chipCollection, Vector3.zero, Quaternion.identity, transform));

            blackJackPhase = BlackJackPhase.Betting;
            blackJackPanel.SetActive(true);
        }

        private void BlackJackBetting()
        {
            blackJackBetting.SetActive(true);
            // Determine whos turn it is
            switch (blackJackPlayerTurn)
            {
                case BlackJackPlayerTurn.Player1:
                    EnablePlaceBetButton(blackJackPlayer1);
                    break;
                case BlackJackPlayerTurn.Player2:
                    EnablePlaceBetButton(blackJackPlayer2);
                    break;
                case BlackJackPlayerTurn.Player3:
                    EnablePlaceBetButton(blackJackPlayer3);
                    break;
            }
        }

        private void EnablePlaceBetButton(BlackJackPlayer player)
        {
            bool enable = true;
            if (betAmount.text.Length == 0)
            {
                enable = false;
            }

            foreach (var ch in betAmount.text)
            {
                foreach (var pc in prohibitedCharacters)
                {
                    if (ch == pc)
                    {
                        enable = false;
                    }
                }
            }
            try
            {
                if (int.Parse(betAmount.text) == 0)
                {
                    enable = false;
                }
            }
            catch
            {
                Debug.LogWarning("int.Parse non numeric value or empty string");
            }

            placebet.interactable = enable;
        }

        public void PlaceBet()
        {
            int bet = int.Parse(betAmount.text);
            blackJackPlayer1.GetBlackJackPlayerChips().SpawnChips(bet);
        }

        private void BlackJackDealing()
        {
            blackJackBetting.SetActive(true);
        }

        private void BlackJackPlayersPlay()
        {
            blackJackPlaying.SetActive(true);
        }

        private void BlackJackDealersPlay()
        {
            blackJackDealer.SetActive(true);
        }

        private void BlackJackSettlement()
        {
            blackJackSettlement.SetActive(true);
        }

        private void BlackJackShuffle()
        {
            blackJackShuffle.SetActive(true);
        }

        private void CreateBlackJackDeck(Difficulty difficulty)
        {
            switch (difficulty)
            {
                case Difficulty.Easy:
                    CreateDeck(1);
                    break;
                case Difficulty.Normal:
                    CreateDeck(2);
                    break;
                case Difficulty.Hard:
                    CreateDeck(6);
                    break;
                case Difficulty.VeryHard:
                    CreateDeck(8);
                    break;
            }
        }
        enum BlackJackPhase
        {
            Betting, Dealing, PlayersPlay, DealersPlay, Settlement, Shuffle
        }

        enum BlackJackPlayerTurn
        {
            Player1, Player2, Player3
        }
    }
}