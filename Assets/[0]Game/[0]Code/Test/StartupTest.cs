using RimuruDev;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game
{
    public class StartupTest : MonoBehaviour
    {
        [SerializeField]
        private PlayerInput _playerInput;

        [SerializeField]
        private CoroutineRunner _coroutineRunner;

        [SerializeField]
        private DeviceTypeDetector _deviceTypeDetector;
        
        private void Awake()
        {
            ServiceLocator.Register(_playerInput);
            ServiceLocator.Register(_coroutineRunner);
            ServiceLocator.Register(_deviceTypeDetector);

            var allMonoBehaviours = FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            
            foreach(var mb in allMonoBehaviours) 
                Injector.Inject(mb);
        }
    }
}