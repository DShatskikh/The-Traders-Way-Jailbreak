using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class PausedScreen : ScreenBase, IGamePauseListener, IGameResumeListener
    {
        [SerializeField]
        private Button _resumeButton;

        private GameStateController _gameStateController;

        [Inject]
        private void Construct(GameStateController gameStateController)
        {
            _gameStateController = gameStateController;
        }
        
        private void OnEnable()
        {
            _resumeButton.onClick.AddListener(_gameStateController.ResumeGame);
        }

        private void OnDisable()
        {
            _resumeButton.onClick.RemoveAllListeners();
        }

        public void OnPauseGame()
        {
            Show();
        }

        public void OnResumeGame()
        {
            Hide();
        }
    }
}