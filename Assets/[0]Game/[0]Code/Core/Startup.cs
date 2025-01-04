using RimuruDev;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game
{
    public sealed class Startup : MonoBehaviour
    {
        [Header("Test Data")]
        [SerializeField]
        private LocationsManager.Data _initializationLocationData;

        [Header("Services")]
        [SerializeField]
        private MusicPlayer _musicPlayer;

        [SerializeField]
        private SoundPlayer _soundPlayer;

        [SerializeField]
        private ScreenManager _screenManager;

        [SerializeField]
        private LocationsManager _locationsManager;

        [SerializeField]
        private WalletService _walletService;

        [SerializeField]
        private VolumeService _volumeService;

        [SerializeField]
        private ADSTimer _adsTimer;

        [SerializeField]
        private ConsoleService _consoleService;
        
        [Header("Links")]
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
        private GameStateController _gameStateController;

        [SerializeField]
        private StockMarketService _stockMarketService;

        [SerializeField]
        private Transform[] _roots;
        
        private readonly TransitionService _transitionService = new();
        
        private void Awake()
        {
            if (FindObjectsByType<Startup>(FindObjectsSortMode.None).Length > 1)
            {
                Destroy(gameObject);
                return;
            }

            DontDestroyOnLoad(gameObject);

            ServiceLocator.Register(_screenManager);
            ServiceLocator.Register(_cinemachineConfiner2D);
            ServiceLocator.Register(_playerInput);
            ServiceLocator.Register(_deviceTypeDetector);
            ServiceLocator.Register(_coroutineRunner);
            ServiceLocator.Register(_player);
            ServiceLocator.Register(_transitionScreen);
            ServiceLocator.Register(_locationsManager);
            ServiceLocator.Register(_gameStateController);
            ServiceLocator.Register(_walletService);
            ServiceLocator.Register(_stockMarketService);
            ServiceLocator.Register(_volumeService);
            ServiceLocator.Register(_transitionService);

            Injector.Inject(_transitionService);
            Injector.Inject(_adsTimer);
            Injector.Inject(_consoleService);
            _gameStateController.AddListener(_adsTimer);
            
            var allMonoBehaviours = FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            
            foreach(var mb in allMonoBehaviours) 
                Injector.Inject(mb);
            
            foreach (var root in _roots)
            {
                foreach (var gameListener in root.GetComponentsInChildren<IGameListener>(true))
                {
                    _gameStateController.AddListener(gameListener);
                }
            }
        }

        private void Start()
        {
            _assetProvider.Init();
            _soundPlayer.Init();
            _musicPlayer.Init();
            _walletService.Init();
            _stockMarketService.Init();
            _locationsManager.Init();
            _volumeService.Init();
            _coroutineRunner.Init();
            CutscenesDataStorage.Init();
            _consoleService.Init();

            var luaCommandRegister = new LuaCommandRegister();
            luaCommandRegister.Register();

            _locationsManager.SwitchLocation(_initializationLocationData.LocationName, _initializationLocationData.PointIndex);

            _gameStateController.StartGame();
        }
    }
}