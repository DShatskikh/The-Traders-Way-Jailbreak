using System;
using RimuruDev;
using UnityEngine;
using UnityEngine.InputSystem;
using DeviceType = RimuruDev.DeviceType;

namespace Game
{
    public sealed class Player : MonoBehaviour, IGameStartListener, IGamePauseListener, IGameResumeListener, 
        IGameUpdateListener,IGameFixedUpdateListener, IGameTransitionListener, IGameLaptopListener,
        IGameShopListener, IGameADSListener, IGameDialogueListener
    {
        [SerializeField]
        private CharacterMover _defaultMover;

        [SerializeField]
        private StepsSoundPlayer _stepsSoundPlayer;

        [SerializeField]
        private CharacterView _view;
        
        [SerializeField]
        private UseAreaChecker _useAreaChecker;

        [SerializeField]
        private CameraAreaChecker _cameraAreaChecker;

        [SerializeField]
        private Joystick _joystick;
        
        private ReactiveProperty<float> _currentSpeed = new();
        private ReactiveProperty<bool> _isRun = new();
        private ReactiveProperty<Vector2> _direction = new();
        
        private PlayerInput _playerInput;
        private Vector3 _previousPosition;
        private bool _isPause;
        private IMover _mover;
        private DeviceTypeDetector _deviceTypeDetector;

        public ReactiveProperty<MonoBehaviour> NearestUseObject => 
            _useAreaChecker.NearestUseObject;

        public Action Move
        {
            get => _mover.MoveAction;
            set => _mover.MoveAction = value;
        }

        public Action StopMove 
        {
            get => _mover.StopAction;
            set => _mover.StopAction = value;
        }

        public bool IsMove => _mover.IsMove;
        public bool IsRun => _mover.IsRun;
        public bool GetFlipX => _view.GetFlipX;
        public Sprite GetSprite => _view.GetSprite;
        public bool IsPause => _isPause;

        [Inject]
        private void Construct(PlayerInput playerInput, DeviceTypeDetector deviceTypeDetector)
        {
            _playerInput = playerInput;
            _deviceTypeDetector = deviceTypeDetector;
        }
        
        private void Awake()
        {
            _mover = _defaultMover;
            
            _cameraAreaChecker.Init();
            _stepsSoundPlayer.Init(transform);
        }

        private void FixedUpdate()
        {
            _cameraAreaChecker.Check();
        }

        public void OnUpdate()
        {
            if (_deviceTypeDetector.DeviceType == DeviceType.WebMobile)
            {
                var direction = _joystick.Direction;
                _isRun.Value = direction.magnitude > 0.95f;

                if (direction.magnitude > 0.2f)
                {
                    _direction.Value = direction.normalized;
                    _mover.Move(_direction.Value,  _isRun.Value);
                }
                else
                {
                    _defaultMover.Stop();
                }
            }
            else
            {
                _isRun.Value = _playerInput.actions["Cancel"].IsPressed();

                if (_playerInput.actions["Move"].IsPressed())
                {
                    _direction.Value = _playerInput.actions["Move"].ReadValue<Vector2>().normalized;
                    _mover.Move(_direction.Value,  _isRun.Value);
                }
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

        public void SetMover(IMover mover)
        {
            _mover = mover;
        }
        
        public void ResetMover()
        {
            _mover = _defaultMover;
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
                _playerInput.actions["Move"].canceled += OnInputMove;
            }
            else
            {
                _currentSpeed.Value = 0;
                _mover.Stop();
                _useAreaChecker.Lost();
                
                _currentSpeed.Changed -= _stepsSoundPlayer.OnSpeedChange;
                _isRun.Changed -= _stepsSoundPlayer.OnIsRunChange;
                _direction.Changed -= _view.OnDirectionChange;
                _currentSpeed.Changed -= _view.OnSpeedChange;
                _playerInput.actions["Move"].canceled -= OnInputMove;
            }
        }

        public void SetViewState(int id)
        {
            if (id == 2)
                _view.Mining();
            else
                _view.OnSpeedChange(0);
        }

        public void SetFlipX(bool isFlipX)
        {
            _view.SetFlipX(isFlipX);
        }

        private void OnInputMove(InputAction.CallbackContext obj)
        {
            _defaultMover.Stop();
        }
    }
}