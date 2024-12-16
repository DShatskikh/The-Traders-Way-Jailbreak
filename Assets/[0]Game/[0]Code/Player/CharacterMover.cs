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

        public void Move(Vector2 direction, bool isRun)
        {
            _rigidbody.linearVelocity = direction * (isRun ? _runSpeed : _speed);
        }
    }
}