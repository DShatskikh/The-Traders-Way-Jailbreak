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
        
        private Player _player;
        private Transform _target;
        private Vector2 _targetPosition;
        private Vector2 _previousPosition;
        private float _currentResetTargetPositionDuration = 0;
        private float _currentFlipDuration = 0;
        private CompanionsManager _companionsManager;

        private static readonly int SpeedHash = Animator.StringToHash("Speed");

        public string GetId => _id;

        [Inject]
        private void Construct(Player player, CompanionsManager companionsManager)
        {
            _player = player;
            _companionsManager = companionsManager;
        }

        private void FixedUpdate()
        {
            _animator.SetFloat(SpeedHash, Vector2.Distance(_previousPosition, transform.position) > 0 ? 1 : 0);
            _previousPosition = transform.position;
        }

        public void Activate(Transform target, Vector2 position, bool isFlip)
        {
            gameObject.SetActive(true);
            transform.position = position;
            _target = target;

            _spriteRenderer.flipX = isFlip;
            
            _player.Move += OnMove;
        }

        public void Deactivate()
        {
            gameObject.SetActive(false);
            
            _player.Move -= OnMove;
        }

        private void OnMove()
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
            
            transform.position -= (Vector3)((Vector2)transform.position - _targetPosition).normalized * _companionsManager.Speed;
        }
    }
}