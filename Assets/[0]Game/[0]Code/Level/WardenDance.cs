using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace Game
{
    public sealed class WardenDance : MonoBehaviour
    {
        [SerializeField]
        private SpriteRenderer _spriteRenderer;

        [SerializeField]
        private Sprite _danceSprite;
        
        private Coroutine _coroutine;
        private bool _isFlipX;
        private float _startY;
        private Sprite _startSprite;
        
        private void Awake()
        {
            _isFlipX = _spriteRenderer.flipX;
            _startY = transform.position.y;

            _startSprite = _spriteRenderer.sprite;
        }

        private void OnEnable()
        {
            StartMove();
        }

        public void StartMove()
        {
            _coroutine = StartCoroutine(AwaitMove());
        }
        
        public void StopMove()
        {
            StopCoroutine(_coroutine);
        }

        private IEnumerator AwaitMove()
        {
            while (true)
            {
                var indexEvent = Random.Range(0, 2);

                switch (indexEvent)
                {
                    case 0:
                        _spriteRenderer.flipX = !_isFlipX;
                        yield return new WaitForSeconds(0.5f);

                        for (int i = 0; i < 2; i++)
                        {
                            _spriteRenderer.sprite = _danceSprite;
                            yield return new WaitForSeconds(0.25f);
                            _spriteRenderer.sprite = _startSprite;
                            yield return new WaitForSeconds(0.25f);
                        }
                        
                        yield return new WaitForSeconds(0.5f);
                        _spriteRenderer.flipX = _isFlipX;
                        yield return new WaitForSeconds(0.5f);
                        
                        for (int i = 0; i < 2; i++)
                        {
                            _spriteRenderer.sprite = _danceSprite;
                            yield return new WaitForSeconds(0.25f);
                            _spriteRenderer.sprite = _startSprite;
                            yield return new WaitForSeconds(0.25f);
                        }
                        
                        break;
                    case 1:
                        int countJump = Random.Range(1, 3);
                        var sequence = DOTween.Sequence();
                        
                        for (int i = 0; i < countJump; i++)
                        {
                            _spriteRenderer.sprite = _startSprite;
                            yield return sequence.Append(_spriteRenderer.transform.DOMoveY(_startY + 1f, 0.75f)).WaitForCompletion();
                            _spriteRenderer.sprite = _danceSprite;
                            yield return sequence.Append(_spriteRenderer.transform.DOMoveY(_startY, 0.75f)).WaitForCompletion();
                            _spriteRenderer.sprite = _startSprite;
                        }
                        
                        break;
                    default:
                        break;
                }
            
                yield return new WaitForSeconds(Random.Range(0.5f, 2f));
            }
        }
    }
}