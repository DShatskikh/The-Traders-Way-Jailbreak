using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Game
{
    public class LaptopScreen : ScreenBase, IGameLaptopListener
    {
        [SerializeField]
        private StockMarketCell _cellPrefab;

        [SerializeField]
        private Transform _container;
        
        [SerializeField]
        private Button _closeButton;
        
        [SerializeField]
        private AudioClip _theme;
        
        private PlayerInput _playerInput;
        private GameStateController _gameStateController;
        private StockMarketService _stockMarketService;
        private AudioClip _previousTheme;

        private void Awake()
        {
            _playerInput = ServiceLocator.Get<PlayerInput>();
            _stockMarketService = ServiceLocator.Get<StockMarketService>();
        }

        private void OnEnable()
        {
            _playerInput.actions["Cancel"].canceled += OnCancel;
        }

        private void OnDisable()
        {
            _playerInput.actions["Cancel"].canceled -= OnCancel;
        }

        public void Start()
        {
            StartCoroutine(AwaitInit());
        }

        public void OnOpenLaptop()
        {
            Show();
            SoundPlayer.Play(AssetProvider.Instance.ClickSound);
            
            _previousTheme = MusicPlayer.Instance.Clip;
            MusicPlayer.Play(_theme);
        }

        public void OnCloseLaptop()
        {
            Hide();
            SoundPlayer.Play(AssetProvider.Instance.ClickSound);
            
            MusicPlayer.Play(_previousTheme);
        }

        public void OnCancel(InputAction.CallbackContext obj)
        {
            _gameStateController.CloseLaptop();
        }

        private IEnumerator AwaitInit()
        {
            foreach (var item in _stockMarketService.GetSlots)
            {
                var cell = Instantiate(_cellPrefab, _container);
                yield return cell.Init(item);
            }
            
            _gameStateController = ServiceLocator.Get<GameStateController>();
            _closeButton.onClick.AddListener(() => OnCancel(new InputAction.CallbackContext()));
        }
    }
}