using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class CharacterView
    {
        [SerializeField] 
        private SpriteRenderer _spriteRenderer;
        
        [SerializeField] 
        private Animator _animator;
        
        private static readonly int StateHash = Animator.StringToHash("State");

        public void OnSpeedChange(float speed)
        {
            _animator.SetFloat(StateHash, (speed > 0) ? 1f : 0f);
        }

        public void OnDirectionChange(Vector2 value)
        {
            if (value.x > 0) 
                Flip(false);
                
            if (value.x < 0) 
                Flip(true);
        }
        
        private void Flip(bool isFlip) => 
            _spriteRenderer.flipX = isFlip;
    }
}