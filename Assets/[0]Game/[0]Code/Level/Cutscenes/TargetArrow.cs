using DG.Tweening;
using UnityEngine;

namespace Game
{
    public class TargetArrow : MonoBehaviour
    {
        [SerializeField]
        private Transform _targetPosition, _arrow;

        private void Start()
        {
            var sequence = DOTween.Sequence();
            sequence
                .Append(_arrow.DOMove(_targetPosition.position, 0.75f))
                //.Append(_arrow.DOMove(_startPosition.position, 1))
                .SetLoops(-1, LoopType.Yoyo);
        }
    }
}