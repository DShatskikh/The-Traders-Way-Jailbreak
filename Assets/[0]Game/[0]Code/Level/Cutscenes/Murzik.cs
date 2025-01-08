using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game
{
    public class Murzik : MonoBehaviour, IGameDialogueListener, IGameFixedUpdateListener
    {
        private static readonly int SpeedHash = Animator.StringToHash("Speed");
        
        private Rigidbody2D _rigidbody;
        private Animator _animator;
        private SpriteRenderer _spriteRenderer;
        private Coroutine _coroutine;

        private Vector2 _previousPosition;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Start()
        {
            _coroutine = StartCoroutine(AwaitMove());
        }
        
        public void OnFixedUpdate()
        {
            if (Vector2.Distance(_previousPosition, transform.position) > 0)
                _animator.SetFloat(SpeedHash, 1);
            else
                _animator.SetFloat(SpeedHash, 0);
            
            _previousPosition = transform.position;
        }

        public void OnShowDialogue()
        {
            if (_coroutine != null)
                StopCoroutine(_coroutine);
            
            _previousPosition = transform.position;
            _animator.SetFloat(SpeedHash, 0);
        }

        public void OnHideDialogue()
        {
            _coroutine = StartCoroutine(AwaitMove());
        }

        private IEnumerator AwaitMove()
        {
            while (true)
            {
                var direction = Random.Range(0, 4) switch
                {
                    0 => new Vector2(1, 0),
                    1 => new Vector2(-1, 0),
                    2 => new Vector2(0, 1),
                    3 => new Vector2(0, -1),
                    _ => throw new ArgumentOutOfRangeException()
                };

                if (direction.x > 0)
                    _spriteRenderer.flipX = false;
                else if (direction.x < 0)
                    _spriteRenderer.flipX = true;
                else
                    _spriteRenderer.flipX = Random.Range(0, 2) == 0;
                
                _rigidbody.linearVelocity = direction * 2;
                yield return new WaitForSeconds(2);
                _rigidbody.linearVelocity = Vector2.zero;
                yield return new WaitForSeconds(1);
            }
        }
    }
}