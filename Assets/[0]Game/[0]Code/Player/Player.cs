using UnityEngine;
using UnityEngine.InputSystem;

namespace Game
{
    public class Player : MonoBehaviour, IResumeGame, IPausedGame, IUpdate, IFixedUpdate
    {
        [SerializeField]
        private CharacterMover _mover;

        [SerializeField]
        private StepsSoundPlayer _stepsSoundPlayer;

        [SerializeField]
        private CharacterView _view;
        
        [SerializeField]
        private UseAreaCheacker _useAreaChecker;

        [SerializeField]
        private CameraAreaChecker _cameraAreaChecker;
        
        private ReactiveProperty<float> _currentSpeed = new();
        private ReactiveProperty<bool> _isRun = new();
        private ReactiveProperty<Vector2> _direction = new();
        
        private PlayerInput _playerInput;
        private Vector3 _previousPosition;

        private void Awake()
        {
            _playerInput = ServiceLocator.Get<PlayerInput>();
            _cameraAreaChecker.Init();
        }

        public void ResumeGame()
        {
            //_currentSpeed.Changed += _stepsSoundPlayer.OnSpeedChange;
            //_isRun.Changed += _stepsSoundPlayer.OnIsRunChange;
            _direction.Changed += _view.OnDirectionChange;
            _currentSpeed.Changed += _view.OnSpeedChange;
            _useAreaChecker.Lost();
            
            _playerInput.actions["Submit"].performed += TryUse;
        }

        public void PausedGame()
        {
            //_currentSpeed.Changed -= _stepsSoundPlayer.OnSpeedChange;
            //_isRun.Changed -= _stepsSoundPlayer.OnIsRunChange;
            _direction.Changed -= _view.OnDirectionChange;
            _currentSpeed.Changed -= _view.OnSpeedChange;
            
            if (_playerInput)
                _playerInput.actions["Submit"].performed -= TryUse;
            
            _mover.Stop();
            _currentSpeed.Value = 0;
        }

        public void OnUpdate()
        {
            _direction.Value = _playerInput.actions["Move"].ReadValue<Vector2>().normalized;
            _isRun.Value = _playerInput.actions["Cancel"].IsPressed();
            _mover.Move(_direction.Value,  _isRun.Value);
        }

        public void OnFixedUpdate()
        {
            var position = transform.position;
            _currentSpeed.Value = ((Vector2)(_previousPosition - position)).magnitude;
            _previousPosition = position;
            _useAreaChecker.Search();
            _cameraAreaChecker.Check();
        }

        private void TryUse(InputAction.CallbackContext obj)
        {
            _useAreaChecker.Use();
        }
    }
}