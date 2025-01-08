using UnityEngine;

namespace Game
{
    public sealed class Companion : MonoBehaviour
    {
        [SerializeField]
        private string _id;

        [SerializeField]
        private SpriteRenderer _spriteRenderer;

        [SerializeField]
        private Animator _animator;
        
        private Transform _target;
        private Vector2 _targetPosition;
        private float _currentResetTargetPositionDuration = 0;
        private float _currentFlipDuration = 0;
        private CompanionsManager _companionsManager;

        private static readonly int SpeedHash = Animator.StringToHash("Speed");

        public string GetId => _id;

        [Inject]
        private void Construct(CompanionsManager companionsManager)
        {
            _companionsManager = companionsManager;
        }

        public void Activate(Transform target, Vector2 position, bool isFlip)
        {
            gameObject.SetActive(true);
            transform.position = position;
            _target = target;

            _spriteRenderer.flipX = isFlip;
        }

        public void Deactivate()
        {
            gameObject.SetActive(false);
        }

        public void OnMove()
        {
            _currentResetTargetPositionDuration += Time.deltaTime;
            _currentFlipDuration += Time.deltaTime;

            if (_currentResetTargetPositionDuration > _companionsManager.ResetTargetPositionDuration)
            {
                _targetPosition = _target.position;
                _currentResetTargetPositionDuration = 0;
            }

            if (_currentFlipDuration > _companionsManager.FlipInterval)
            {
                _spriteRenderer.flipX = _targetPosition.x - transform.position.x < 0;
                _currentFlipDuration = 0;
            }
            
            transform.position -= (Vector3)((Vector2)transform.position - _targetPosition).normalized * _companionsManager.GetSpeed * Time.deltaTime;
            _animator.SetFloat(SpeedHash, 1);
        }

        public void OnStop()
        {
            _animator.SetFloat(SpeedHash, 0);
        }
    }
}