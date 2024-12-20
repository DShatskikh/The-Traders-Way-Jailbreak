using RimuruDev;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game
{
    public sealed class Startup : MonoBehaviour
    {
        [SerializeField]
        private MusicPlayer _musicPlayer;

        [SerializeField]
        private SoundPlayer _soundPlayer;

        [SerializeField]
        private ScreenManager _screenManager;

        [SerializeField]
        private CinemachineConfiner2D _cinemachineConfiner2D;

        [SerializeField]
        private PlayerInput _playerInput;

        [SerializeField]
        private DeviceTypeDetector _deviceTypeDetector;

        [SerializeField]
        private AssetProvider _assetProvider;

        [SerializeField]
        private CoroutineRunner _coroutineRunner;

        [SerializeField]
        private Player _player;

        [SerializeField]
        private TransitionScreen _transitionScreen;

        [SerializeField]
        private LocationsManager _locationsManager;

        [SerializeField]
        private LocationsManager.Data _initializationLocationData;

        [SerializeField]
        private GameStateController _game;

        [SerializeField]
        private WalletService _walletService;

        [SerializeField]
        private StockMarketService _stockMarketService;

        [SerializeField]
        private VolumeService _volumeService;
        
        private void Awake()
        {
            if (FindObjectsByType<Startup>(FindObjectsSortMode.None).Length > 1)
            {
                Destroy(gameObject);
                return;
            }

            DontDestroyOnLoad(gameObject);

            _assetProvider.Init();
            _soundPlayer.Init();
            _musicPlayer.Init();
            
            ServiceLocator.Register(_screenManager);
            ServiceLocator.Register(_cinemachineConfiner2D);
            ServiceLocator.Register(_playerInput);
            ServiceLocator.Register(_deviceTypeDetector);
            ServiceLocator.Register(_coroutineRunner);
            ServiceLocator.Register(_player);
            ServiceLocator.Register(_transitionScreen);
            ServiceLocator.Register(_locationsManager);
            ServiceLocator.Register(_game);
            ServiceLocator.Register(_walletService);
            ServiceLocator.Register(_stockMarketService);
            ServiceLocator.Register(_volumeService);
        }

        private void Start()
        {
            _stockMarketService.Init();
            _locationsManager.Init();
            _volumeService.Init();
            _locationsManager.SwitchLocation(_initializationLocationData.LocationName, _initializationLocationData.PointIndex);

            _game.StartGame();
        }
    }
}