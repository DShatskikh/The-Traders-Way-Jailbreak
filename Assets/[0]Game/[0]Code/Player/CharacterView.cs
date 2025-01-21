using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public sealed class CharacterView
    {
        private static readonly int StateHash = Animator.StringToHash("State");

        [SerializeField] 
        private SpriteRenderer _spriteRenderer;

        [SerializeField] 
        private Animator _animator;

        [SerializeField]
        private HatView _hatView;

        public bool GetFlipX => _spriteRenderer.flipX;
        public Sprite GetSprite => _spriteRenderer.sprite;

        public void OnSpeedChange(float speed)
        {
            _animator.SetFloat(StateHash, (speed > 0) ? 1f : 0f);
        }

        public void OnDirectionChange(Vector2 value)
        {
            if (value.x > 0) 
                SetFlipX(false);
                
            if (value.x < 0) 
                SetFlipX(true);
        }

        public void Mining()
        {
            _animator.SetFloat(StateHash, 2);
        }

        public void SetFlipX(bool isFlip)
        {
            _spriteRenderer.flipX = isFlip;
            _hatView.HatPoint.transform.localScale = _hatView.HatPoint.transform.localScale.SetX(isFlip ? -1 : 1);
        }
    }
}