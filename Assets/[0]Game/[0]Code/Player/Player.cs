using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game
{
    public class Player : MonoBehaviour, IGameStartListener, IGamePauseListener, IGameResumeListener, 
        IGameUpdateListener,IGameFixedUpdateListener, IGameTransitionListener, IGameLaptopListener,
        IGameShopListener, IGameADSListener, IGameDialogueListener
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
        private bool _isPause;

        public ReactiveProperty<MonoBehaviour> NearestUseObject => 
            _useAreaChecker.NearestUseObject;

        public Action Move
        {
            get { return _mover.MoveAction; }
            set { _mover.MoveAction = value; }
        }

        public bool IsMove => _mover.IsMove;
        
        [Inject]
        private void Construct(PlayerInput playerInput)
        {
            _playerInput = playerInput;
        }
        
        private void Awake()
        {
            _cameraAreaChecker.Init();
            _stepsSoundPlayer.Init(transform);
        }

        private void FixedUpdate()
        {
            _cameraAreaChecker.Check();
        }

        public void OnUpdate()
        {
            _isRun.Value = _playerInput.actions["Cancel"].IsPressed();

            if (_playerInput.actions["Move"].IsPressed())
            {
                _direction.Value = _playerInput.actions["Move"].ReadValue<Vector2>().normalized;
                _mover.Move(_direction.Value,  _isRun.Value);
            }
        }

        public void OnFixedUpdate()
        {
            var position = transform.position;
            _currentSpeed.Value = ((Vector2)(_previousPosition - position)).magnitude;
            _previousPosition = position;

            if (!_isPause)
            {
                _useAreaChecker.Search();
            }
        }

        public void OnStartGame()
        {
            Activate(true);
        }

        public void OnPauseGame()
        {
            Activate(false);
        }

        public void OnResumeGame()
        {
            Activate(true);
        }

        public void OnStartTransition()
        {
            Activate(false);
        }

        public void OnEndTransition()
        {
            Activate(true);
        }

        public void OnOpenLaptop()
        {
            Activate(false);
        }

        public void OnCloseLaptop()
        {
            Activate(true);
        }

        public void OnOpenShop()
        {
            Activate(false);
        }

        public void OnCloseShop()
        {
            Activate(true);
        }

        public void OnShowADS()
        {
            Activate(false);
        }

        public void OnHideADS()
        {
            Activate(true);
        }
        
        public void OnShowDialogue()
        {
            Activate(false);
        }

        public void OnHideDialogue()
        {
            Activate(true);
        }
        
        public void TryUse(InputAction.CallbackContext obj)
        {
            _useAreaChecker.Use();
        }

        private void Activate(bool isActivate)
        {
            _isPause = !isActivate;
            
            if (isActivate)
            {
                _previousPosition = transform.position;
                
                _currentSpeed.Changed += _stepsSoundPlayer.OnSpeedChange;
                _isRun.Changed += _stepsSoundPlayer.OnIsRunChange;
                _direction.Changed += _view.OnDirectionChange;
                _currentSpeed.Changed += _view.OnSpeedChange;
                _useAreaChecker.Lost();
            
                _playerInput.actions["Submit"].canceled += TryUse;
                _playerInput.actions["Move"].canceled += (_) => _mover.Stop();
            }
            else
            {
                _currentSpeed.Changed -= _stepsSoundPlayer.OnSpeedChange;
                _isRun.Changed -= _stepsSoundPlayer.OnIsRunChange;
                
                if (_playerInput)
                    _playerInput.actions["Submit"].canceled -= TryUse;
            
                _mover.Stop();
                _currentSpeed.Value = 0;
                
                _direction.Changed -= _view.OnDirectionChange;
                _currentSpeed.Changed -= _view.OnSpeedChange;
                _playerInput.actions["Move"].canceled -= (_) => _mover.Stop();
                
                _useAreaChecker.Lost();
            }
        }
    }
}