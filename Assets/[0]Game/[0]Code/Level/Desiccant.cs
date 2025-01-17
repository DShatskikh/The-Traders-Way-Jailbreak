using DG.Tweening;
using UnityEngine;

namespace Game
{
    public class Desiccant : MonoBehaviour
    {
        private Sequence _sequence;
        
        private void Start()
        {
            _sequence?.Kill();
            _sequence = DOTween.Sequence();

            _sequence
                .Append(transform.DOMoveY(transform.position.y + 0.2f, 1.5f))
                .SetLoops(-1, LoopType.Yoyo);
        }

        private void OnDestroy()
        {
            _sequence?.Kill();
        }
    }
}