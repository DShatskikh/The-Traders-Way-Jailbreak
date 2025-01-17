using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game
{
    public sealed class StartGameScreenBackground : MonoBehaviour
    {
        [SerializeField]
        private Animator _animator;

        [SerializeField]
        private SpriteRenderer _spriteRenderer;
        
        [SerializeField]
        private Transform _rocket;

        [SerializeField]
        private Transform[] _rocketStartPoints;

        [SerializeField]
        private Transform _luckyBlock;

        private Sequence _sequence;
        private Sequence _rocketSequence;

        private void Start()
        {
            StartCoroutine(AwaitPlayerAnimation());
            StartCoroutine(AwaitRocketAnimation());
            StartCoroutine(AwaitLuckyBlockAnimation());
        }
        
        private IEnumerator AwaitPlayerAnimation()
        {
            var states = new[] { 0, 1, 3 };
            
            while (true)
            {
                yield return new WaitForSeconds(2);
                var state = Random.Range(0, states.Length);
                _animator.SetFloat("State", state);
                _spriteRenderer.flipX = Random.Range(0, 2) == 0;
            }
        }

        private IEnumerator AwaitRocketAnimation()
        {
            while (true)
            {
                yield return new WaitForSeconds(3);
                _rocket.gameObject.SetActive(true);

                var rocketStartPoint = _rocketStartPoints[Random.Range(0, _rocketStartPoints.Length)];
                _rocket.SetParent(rocketStartPoint);
                
                _rocket.position = rocketStartPoint.position;
                _rocket.rotation = rocketStartPoint.rotation;

                _rocketSequence?.Kill();
                _rocketSequence = DOTween.Sequence();
                yield return _rocketSequence.Append(_rocket.DOLocalMoveY(_rocket.localPosition.y + 30, 6)).WaitForCompletion();
                _rocket.gameObject.SetActive(false);
            }
        }

        private IEnumerator AwaitLuckyBlockAnimation()
        {
            while (true)
            {
                _sequence?.Kill();
                _sequence = DOTween.Sequence();
                yield return _sequence
                    .Append(_luckyBlock.DOLocalMoveY(_luckyBlock.localPosition.y + 10, 20))
                    .Insert(0, _luckyBlock.DORotate(new Vector3(0, 0, _luckyBlock.localRotation.z + 90), 20))
                    .WaitForCompletion();

                if (_luckyBlock.transform.position.y > 10)
                    _luckyBlock.transform.position = _luckyBlock.transform.position.SetY(-10);
            }
        }

        private void OnDestroy()
        {
            _sequence?.Kill();
            _rocketSequence?.Kill();
        }
    }
}