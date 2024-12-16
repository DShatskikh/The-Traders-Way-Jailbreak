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
        
        private void Awake()
        {
            if (FindObjectsByType<Startup>(FindObjectsSortMode.None).Length > 1)
            {
                Destroy(gameObject);
                return;
            }

            DontDestroyOnLoad(gameObject);

            DIContainer.Register(_musicPlayerService);
            DIContainer.Register(_soundPlayerService);
            DIContainer.Register(_screenManager);
            DIContainer.Register(_cinemachineConfiner2D);
            DIContainer.Register(_playerInput);
        }
    }
}