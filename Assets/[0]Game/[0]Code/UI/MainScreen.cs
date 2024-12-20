using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class MainScreen : MonoBehaviour, IPausedGame, IResumeGame
    {
        [SerializeField]
        private Button _pauseButton;

        [SerializeField]
        private TMP_Text _moneyLabel;
        
        public void PausedGame()
        {
            _pauseButton.gameObject.SetActive(false);
            _moneyLabel.gameObject.SetActive(false);
        }

        public void ResumeGame()
        {
            _pauseButton.gameObject.SetActive(true);
            _moneyLabel.gameObject.SetActive(true);
        }
    }
}