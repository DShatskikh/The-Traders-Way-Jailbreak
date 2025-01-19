using PixelCrushers.DialogueSystem;
using RimuruDev;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game
{
    public sealed class Startup : MonoBehaviour
    {
        [SerializeField]
        private bool _fullTest;
        
        [Header("Test Data")]
        [SerializeField]
        private LocationsManager.Data _initializationLocationData;

        [SerializeField]
        private bool _isOpenStockMarket;

        [SerializeField]
        private bool _isShowMenu;

        [SerializeField]
        private HomeCutscene.SaveData _homeData;

        [SerializeField]
        private MyCellCutscene.SaveData _myCellData;

        [SerializeField]
        private NoobikSkinShop.SaveData _noobikData;

        [SerializeField]
        private EndsData _endsData;
        
        [SerializeField]
        private VolumeData _volume;
        
        [SerializeField]
        private double _startMoney = 999999999;

        [SerializeField]
        private double _startTax = 999999999;
        
        [SerializeField]
        private string _playerName = "Денис";
        
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

        [SerializeField]
        private DialogueExtensions _dialogueExtensions;

        [SerializeField]
        private CompanionsManager _companionsManager;
        
        [SerializeField]
        private HatManager _hatManager;

        [SerializeField]
        private AllBuyCheckHandler _allBuyCheckHandler;
        
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
        private readonly PurchasedManager _purchasedManager = new();
        private readonly AdsManager _adsManager = new();
        private readonly LocationLoader _locationLoader = new();
        private readonly OpenMainMenuHandler _openMainMenuHandler = new();
        private readonly EndingsGame _endingsGame = new();
        private readonly LuaCommandRegister _luaCommandRegister = new();
        private readonly IAnalyticsService _analyticsService = new YandexAnalytics();
        
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
            ServiceLocator.Register(_companionsManager);
            ServiceLocator.Register(_hatManager);
            ServiceLocator.Register(_locationLoader);
            ServiceLocator.Register(_endingsGame);
            ServiceLocator.Register(_allBuyCheckHandler);
            ServiceLocator.Register(_analyticsService);

            Injector.Inject(_transitionService);
            Injector.Inject(_adsTimer);
            Injector.Inject(_consoleService);
            Injector.Inject(_companionsManager);
            Injector.Inject(_purchasedManager);
            Injector.Inject(_adsManager);
            Injector.Inject(_locationLoader);
            Injector.Inject(_openMainMenuHandler);
            Injector.Inject(_endingsGame);
            Injector.Inject(_luaCommandRegister);
            Injector.Inject(_allBuyCheckHandler);
            Injector.Inject(_hatManager);
            
            _gameStateController.AddListener(_adsTimer);
            _gameStateController.AddListener(_dialogueExtensions);
            _gameStateController.AddListener(_companionsManager);
            _gameStateController.AddListener(_openMainMenuHandler);
            
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
            _coroutineRunner.Init();
            RepositoryStorage.Init();
            _consoleService.Init();
            _dialogueExtensions.Init();
            _companionsManager.Init();
            _hatManager.Init();
            _luaCommandRegister.Init();

#if UNITY_EDITOR
            if (!_fullTest)
            {
                RepositoryStorage.Set(KeyConstants.HomeCutscene, _homeData);
                RepositoryStorage.Set(KeyConstants.MyCellCutscene, _myCellData);
                RepositoryStorage.Set(KeyConstants.SkinShop, _noobikData);
                RepositoryStorage.Set(KeyConstants.Ending, _endsData);
                RepositoryStorage.Set(KeyConstants.Volume, _volume);
            
                _walletService.SetMoneyAndTax(_startMoney, _startTax);
                
                if (_isOpenStockMarket)
                    _stockMarketService.OpenAllItems();
            }
#endif

            _volumeService.Init();
            
#if UNITY_EDITOR
            if (!_fullTest)
            {
                RepositoryStorage.Set(KeyConstants.Name, new PlayerName(_playerName));
                Lua.Run($"Variable[\"PlayerName\"] = \"{_playerName}\"");
                
                if (_isShowMenu)
                {
                    _gameStateController.OpenMainMenu();
                }
                else
                {
                    _locationsManager.SwitchLocation(_initializationLocationData.LocationName, _initializationLocationData.PointIndex);   
                    _gameStateController.StartGame();
                }
            }
            else
            {
                _gameStateController.OpenMainMenu();
            }
#else
                _locationsManager.SwitchLocation("World", 0);
#endif

        }
    }
}