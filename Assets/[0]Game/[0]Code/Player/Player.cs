using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game
{
    public class Player : MonoBehaviour, ICameraAreaChecker
    {
        [SerializeField]
        private CharacterMover _mover;

        [SerializeField]
        private StepsSoundPlayer _stepsSoundPlayer;

        [SerializeField]
        private CharacterView _view;

        [SerializeField]
        private TriggerChecker _checkArea;
        
        private ReactiveProperty<float> _currentSpeed = new();
        private ReactiveProperty<bool> _isRun = new();
        private ReactiveProperty<Vector2> _direction = new();
        
        private PlayerInput _playerInput;
        private Vector3 _previousPosition;

        private void Awake()
        {
            _playerInput = DIContainer.Get<PlayerInput>();
        }

        private void OnEnable()
        {
            //_currentSpeed.Changed += _stepsSoundPlayer.OnSpeedChange;
            //_isRun.Changed += _stepsSoundPlayer.OnIsRunChange;
            _direction.Changed += _view.OnDirectionChange;
            _currentSpeed.Changed += _view.OnSpeedChange;
            _checkArea.TriggerEnter = OnAreaTriggerEnter;
        }

        private void OnAreaTriggerEnter(GameObject obj)
        {
            if (obj.TryGetComponent(out IUsable usable))
            {
                
            }
        }

        private void OnDisable()
        {
            //_currentSpeed.Changed -= _stepsSoundPlayer.OnSpeedChange;
            //_isRun.Changed -= _stepsSoundPlayer.OnIsRunChange;
            _direction.Changed -= _view.OnDirectionChange;
            _currentSpeed.Changed -= _view.OnSpeedChange;
        }
        
        private void Update()
        {
            _direction.Value = _playerInput.actions["Move"].ReadValue<Vector2>().normalized;
            _isRun.Value = _playerInput.actions["Cancel"].IsPressed();
            _mover.Move(_direction.Value,  _isRun.Value);
        }

        private void FixedUpdate()
        {
            var position = transform.position;
            _currentSpeed.Value = ((Vector2)(_previousPosition - position)).magnitude;
            _previousPosition = position;
        }
    }
}