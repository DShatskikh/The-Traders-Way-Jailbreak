using UnityEngine;
using UnityEngine.InputSystem;

namespace Game
{
    public class UseButton : MonoBehaviour, IGameStartListener
    {
        private Player _player;
        private PlayerInput _playerInput;
        private AnimatedButton _button;

        [Inject]
        private void Construct(Player player, PlayerInput playerInput)
        {
            _player = player;
            _playerInput = playerInput;
        }

        private void Awake()
        {
            _button = GetComponent<AnimatedButton>();
        }

        private void OnEnable()
        {
            _playerInput.actions["Submit"].started += Onstarted;
            _playerInput.actions["Submit"].canceled += Oncanceled;
            
            _button.onClick.AddListener(() => _player.TryUse(new InputAction.CallbackContext()));
        }

        private void OnDisable()
        {
            if (_playerInput)
            {
                _playerInput.actions["Submit"].started -= Onstarted;
                _playerInput.actions["Submit"].canceled -= Oncanceled; 
                
                _button.onClick.RemoveAllListeners();
            }
        }

        public void OnStartGame()
        {
            _player.NearestUseObject.Changed += NearestUseObjectOnChanged;
        }

        private void NearestUseObjectOnChanged(MonoBehaviour obj)
        {
            gameObject.SetActive(obj != null);
        }

        private void Onstarted(InputAction.CallbackContext obj)
        {
            _button.OnPointerDown(null);
        }

        private void Oncanceled(InputAction.CallbackContext obj)
        {
            _button.OnPointerUp(null);
            _button.onClick.Invoke();
            //_player.TryUse(new InputAction.CallbackContext());
            gameObject.SetActive(false);
        }
    }
}