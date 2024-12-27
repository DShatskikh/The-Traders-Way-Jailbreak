using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class MainScreen : MonoBehaviour, IGameStartListener, IGameLaptopListener, IGamePauseListener, IGameResumeListener
    {
        [SerializeField]
        private Button _pauseButton;

        [SerializeField]
        private TMP_Text _moneyLabel;

        private void Activate(bool isActive)
        {
            if (isActive)
            {
                _pauseButton.gameObject.SetActive(true);
                _moneyLabel.gameObject.SetActive(true); 
            }
            else
            {
                _pauseButton.gameObject.SetActive(false);
                _moneyLabel.gameObject.SetActive(false);
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
    }
}