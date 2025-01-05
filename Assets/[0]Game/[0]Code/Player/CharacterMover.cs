using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class CharacterMover
    {
        [SerializeField]
        private float _speed = 3;
        
        [SerializeField]
        private float _runSpeed = 7;
        
        [SerializeField]
        private Rigidbody2D _rigidbody;

        private bool _isMove;
        
        public Action MoveAction;
        public bool IsMove => _isMove;
        
        public void Move(Vector2 direction, bool isRun)
        {
            _rigidbody.linearVelocity = direction * (isRun ? _runSpeed : _speed);
            MoveAction?.Invoke();
            _isMove = true;
        }

        public void Stop()
        {
            _rigidbody.linearVelocity = Vector2.zero;
            _isMove = false;
        }
    }
}