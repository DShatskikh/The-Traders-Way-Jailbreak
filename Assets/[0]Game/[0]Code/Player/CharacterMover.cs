using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public sealed class CharacterMover : IMover
    {
        [SerializeField]
        private float _speed = 3;
        
        [SerializeField]
        private float _runSpeed = 7;
        
        [SerializeField]
        private Rigidbody2D _rigidbody;

        private bool _isMove;
        private bool _isRun;

        public Action MoveAction { get; set; }
        public Action StopAction { get; set; }

        public bool IsMove => _isMove;
        public bool IsRun => _isRun;

        public void Move(Vector2 direction, bool isRun)
        {
            _isRun = isRun;
            _rigidbody.linearVelocity = direction * (isRun ? _runSpeed : _speed);
            MoveAction?.Invoke();
            _isMove = true;
        }

        public void Stop()
        {
            _rigidbody.linearVelocity = Vector2.zero;
            _isMove = false;
            StopAction?.Invoke();
        }
    }

    public interface IMover
    {
        Action MoveAction { get; set; }
        Action StopAction { get; set; }
        bool IsMove { get; }
        bool IsRun { get; }
        void Move(Vector2 directionValue, bool isRunValue);
        void Stop();
    }

    public class CharacterLadderMover : IMover
    {
        private readonly Transform _transform;
        private readonly Vector2 _startPosition;
        private readonly Vector2 _targetPosition;
        private float _progress;
        private readonly bool _isRight;
        public Action MoveAction { get; set; }
        public Action StopAction { get; set; }
        public bool IsMove { get; }
        public bool IsRun => _isRight;

        public CharacterLadderMover(Transform transform, Vector2 startPosition, Vector2 targetPosition, bool isRight)
        {
            _transform = transform;
            _startPosition = startPosition;
            _targetPosition = targetPosition;
            _isRight = isRight;

            _progress = _isRight ? 1 : 0;

            if (_isRight)
                _targetPosition.y = transform.position.y;
            else
                _startPosition.y = transform.position.y;
        }

        public void Move(Vector2 directionValue, bool isRunValue)
        {
            var speed = isRunValue ? 2 : 1;
            
            if (directionValue.x < 0)
            {
                _progress += Time.deltaTime * speed;

                if (_progress > 1)
                    _progress = 1;
                
                _transform.position = Vector2.Lerp(_startPosition, _targetPosition, _progress);
            }
            else if (directionValue.x > 0)
            {
                _progress -= Time.deltaTime * speed;
                
                if (_progress < 0)
                    _progress = 0;
                
                _transform.position = Vector2.Lerp(_startPosition, _targetPosition, _progress);
            }
            
            MoveAction?.Invoke();
        }

        public void Stop()
        {
            StopAction?.Invoke();
        }
    }
}