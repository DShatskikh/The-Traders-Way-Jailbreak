using DG.Tweening;
using UnityEngine;

namespace Game
{
    public class Slime : MonoBehaviour
    {
        private Sequence _sequence;
        
        private void Start()
        {
            _sequence?.Kill();
            _sequence = DOTween.Sequence();

            _sequence
                .Append(transform.DOScaleY(0.9f, 1))
                .SetLoops(-1, LoopType.Yoyo);
        }

        private void OnDestroy()
        {
            _sequence?.Kill();
        }
    }
}