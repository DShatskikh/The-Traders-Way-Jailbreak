using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public sealed class MainScreen : ScreenBase, IGameStartListener, IGameLaptopListener, IGamePauseListener, IGameResumeListener
    {
        [SerializeField]
        private Button _pauseButton;

        [SerializeField]
        private TMP_Text _moneyLabel;

        private GameStateController _gameStateController;

        [Inject]
        private void Construct(GameStateController gameStateController)
        {
            _gameStateController = gameStateController;
        }
        
        private void Activate(bool isActive)
        {
            if (isActive)
            {
                _pauseButton.gameObject.SetActive(true);
                _moneyLabel.gameObject.SetActive(true); 
                
                _pauseButton.onClick.AddListener(PauseClick);
            }
            else
            {
                _pauseButton.gameObject.SetActive(false);
                _moneyLabel.gameObject.SetActive(false);
                
                _pauseButton.onClick.RemoveAllListeners();
            }
        }

        public void OnStartGame()
        {
            Activate(true);
        }

        public void OnOpenLaptop()
        {
            Activate(false);
        }

        public void OnCloseLaptop()
        {
            Activate(true);
        }

        public void OnPauseGame()
        {
            Activate(false);
        }

        public void OnResumeGame()
        {
            Activate(true);
        }

        private void PauseClick()
        {
            _gameStateController.PauseGame();
        }
    }
}