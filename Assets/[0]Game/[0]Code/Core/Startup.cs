using System.Collections;
using System.Linq;
using PixelCrushers.DialogueSystem;
using RimuruDev;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Localization.Settings;
using YG;
using YG.Insides;

namespace Game
{
    public sealed class Startup : MonoBehaviour
    {
        [SerializeField]
        private bool _fullTest;

        [Header("Test Data")]
        [SerializeField]
        private bool _isUseSaving;
        
        [SerializeField]
        private AllInitData _initData;
        
        [SerializeField]
        private bool _isOpenStockMarket;

        [SerializeField]
        private bool _isShowMenu;
        
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

        [SerializeField]
        private SaveLoadService _saveLoadService;
        
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
        private IAnalyticsService _analyticsService;

        private void Awake()
        {
            if (FindObjectsByType<Startup>(FindObjectsSortMode.None).Length > 1)
            {
                Destroy(gameObject);
                return;
            }

            DontDestroyOnLoad(gameObject);

#if PLATFORM_WEBGL
            _analyticsService = new YandexAnalytics();
#else
            _analyticsService = new TestAnalyticsService();
#endif

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

            ISaveLoadService saveLoadService = _saveLoadService;
            ServiceLocator.Register(saveLoadService);

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
            Injector.Inject(saveLoadService);
            Injector.Inject(_locationsManager);

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
            
            YGInsides.LoadProgress();
            YG2.onGetSDKData += Load;
        }
        
        private void Load()
        {
            YG2.onGetSDKData -= Load;
            CorrectLang.OnСhangeLang(YG2.lang);
            
#if UNITY_EDITOR
            if (!_fullTest)
            {
                if (!_isUseSaving)
                {
                    print("Full Test");
                    RepositoryStorage.Set(KeyConstants.HomeCutscene, _initData.HomeData);
                    RepositoryStorage.Set(KeyConstants.MyCellCutscene, _initData.MyCellData);
                    RepositoryStorage.Set(KeyConstants.SkinShop, _initData.NoobikData);
                    RepositoryStorage.Set(KeyConstants.Ending, _initData.EndsData);
                    RepositoryStorage.Set(KeyConstants.Volume, _initData.Volume);
                    RepositoryStorage.Set(KeyConstants.StockMarket, new StockMarketService.Data());
                }
            }
            else
            {
                RepositoryStorage.Set(KeyConstants.IsNotFirstOpen, new FirstOpen { IsNotFirstOpen = false });
            }
#else
            
#endif
            
            _assetProvider.Init();
            _soundPlayer.Init();
            _musicPlayer.Init();
            _walletService.Init();
            _stockMarketService.Init();
            _locationsManager.Init();
            _coroutineRunner.Init();
            _consoleService.Init();
            _dialogueExtensions.Init();
            _companionsManager.Init();
            _hatManager.Init();
            _luaCommandRegister.Init();
            _volumeService.Init();

#if UNITY_EDITOR
            if (!_fullTest)
            {
                if (_isUseSaving)
                {
                    _gameStateController.OpenMainMenu();
                    return;
                }
                
                _walletService.SetMoneyAndTax(_initData.StartMoney, _initData.StartTax);
                
                if (_isOpenStockMarket)
                    _stockMarketService.OpenAllItems(); 
                
                RepositoryStorage.Set(KeyConstants.Name, new PlayerName(_initData.PlayerName));
                Lua.Run($"Variable[\"PlayerName\"] = \"{_initData.PlayerName}\"");
                
                if (_isShowMenu)
                {
                    _gameStateController.OpenMainMenu();
                }
                else
                {
                    _locationsManager.SwitchLocation(_initData.LocationData.LocationName, _initData.LocationData.PointIndex);   
                    _gameStateController.StartGame();
                }
            }
            else
            {
                _gameStateController.OpenMainMenu();
            }
#else
            _gameStateController.OpenMainMenu();
#endif
        }
    }
}