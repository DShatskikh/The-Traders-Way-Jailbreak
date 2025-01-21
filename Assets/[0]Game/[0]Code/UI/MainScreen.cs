using RimuruDev;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DeviceType = RimuruDev.DeviceType;

namespace Game
{
    public sealed class MainScreen : ScreenBase, IGameStartListener, IGameLaptopListener, IGamePauseListener, IGameResumeListener
    {
        [SerializeField]
        private Button _pauseButton;

        [SerializeField]
        private TMP_Text _moneyLabel;

        [SerializeField]
        private JoystickView _joystick;
        
        [SerializeField]
        private Button _runButton;
        
        private GameStateController _gameStateController;
        private DeviceTypeDetector _deviceTypeDetector;

        [Inject]
        private void Construct(GameStateController gameStateController, DeviceTypeDetector deviceTypeDetector)
        {
            _gameStateController = gameStateController;
            _deviceTypeDetector = deviceTypeDetector;
        }
        
        private void Activate(bool isActive)
        {
            if (isActive)
            {
                _pauseButton.gameObject.SetActive(true);
                _moneyLabel.gameObject.SetActive(true); 
                
                _pauseButton.onClick.AddListener(PauseClick);

                if (_deviceTypeDetector.DeviceType == DeviceType.WebMobile)
                {
                    _runButton.gameObject.SetActive(true);
                    _joystick.gameObject.SetActive(true);
                }
            }
            else
            {
                _pauseButton.gameObject.SetActive(false);
                _moneyLabel.gameObject.SetActive(false);
                
                _pauseButton.onClick.RemoveAllListeners();

                if (_deviceTypeDetector.DeviceType == DeviceType.WebMobile)
                {
                    _runButton.gameObject.SetActive(false);
                    _joystick.gameObject.SetActive(false);
                }
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