using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public sealed class PausedScreen : ScreenBase, IGamePauseListener, IGameResumeListener
    {
        [SerializeField]
        private Button _resumeButton, _menuButton;

        private GameStateController _gameStateController;

        [Inject]
        private void Construct(GameStateController gameStateController)
        {
            _gameStateController = gameStateController;
        }
        
        private void OnEnable()
        {
            _resumeButton.onClick.AddListener(_gameStateController.ResumeGame);
            _menuButton.onClick.AddListener(OnMenuButtonClicked);
        }

        private void OnDisable()
        {
            _resumeButton.onClick.RemoveAllListeners();
            _menuButton.onClick.RemoveAllListeners();
        }

        public void OnPauseGame()
        {
            Show();
        }

        public void OnResumeGame()
        {
            Hide();
        }

        private void OnMenuButtonClicked()
        {
            _gameStateController.OpenMainMenu();
        }
    }
}