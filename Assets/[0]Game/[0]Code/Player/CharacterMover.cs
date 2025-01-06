using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class CharacterMover : IMover
    {
        [SerializeField]
        private float _speed = 3;
        
        [SerializeField]
        private float _runSpeed = 7;
        
        [SerializeField]
        private Rigidbody2D _rigidbody;

        public Action MoveAction { get; set; }

        public void Move(Vector2 direction, bool isRun)
        {
            _rigidbody.linearVelocity = direction * (isRun ? _runSpeed : _speed);
            MoveAction?.Invoke();
        }

        public void Stop()
        {
            _rigidbody.linearVelocity = Vector2.zero;
        }
    }

    public interface IMover
    {
        Action MoveAction { get; set; }
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

        public CharacterLadderMover(Transform transform, Vector2 startPosition, Vector2 targetPosition, bool isRight)
        {
            _transform = transform;
            _startPosition = startPosition;
            _targetPosition = targetPosition;
            _isRight = isRight;

            _progress = _isRight ? 1 : 0;
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
            
        }
    }
}