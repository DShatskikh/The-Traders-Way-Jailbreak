using RimuruDev;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game
{
    public sealed class Startup : MonoBehaviour
    {
        [SerializeField]
        private MusicPlayerService _musicPlayerService;

        [SerializeField]
        private SoundPlayerService _soundPlayerService;

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
        
        private void Awake()
        {
            if (FindObjectsByType<Startup>(FindObjectsSortMode.None).Length > 1)
            {
                Destroy(gameObject);
                return;
            }

            DontDestroyOnLoad(gameObject);

            ServiceLocator.Register(_musicPlayerService);
            ServiceLocator.Register(_soundPlayerService);
            ServiceLocator.Register(_screenManager);
            ServiceLocator.Register(_cinemachineConfiner2D);
            ServiceLocator.Register(_playerInput);
            ServiceLocator.Register(_assetProvider);
            ServiceLocator.Register(_deviceTypeDetector);
            ServiceLocator.Register(_coroutineRunner);
            ServiceLocator.Register(_player);
            ServiceLocator.Register(_transitionScreen);
            ServiceLocator.Register(_locationsManager);
        }

        private void Start()
        {
            _locationsManager.Init();
            _locationsManager.SwitchLocation(_initializationLocationData.LocationName, _initializationLocationData.PointIndex);
        }
    }
}