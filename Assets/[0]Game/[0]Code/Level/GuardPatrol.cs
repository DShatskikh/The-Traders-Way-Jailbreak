﻿using System.Collections;
using UnityEngine;

namespace Game
{
    public sealed class GuardPatrol : MonoBehaviour, IGameTransitionListener
    {
        [SerializeField]
        private TriggerChecker _warningZone;

        [SerializeField]
        private Transform _warningZoneTransform;
        
        [SerializeField]
        private Transform _startPoint;

        [SerializeField]
        private Transform _targetPoint;
        
        [SerializeField]
        private Transform _guard;

        [SerializeField]
        private Animator _animator;

        [SerializeField]
        private SpriteRenderer _spriteRenderer;
        
        private TransitionService _transitionService;
        private Coroutine _coroutine;

        [Inject]
        private void Construct(TransitionService transitionService)
        {
            _transitionService = transitionService;
        }

        private void Start()
        {
            StartPatrol();
        }
        
        public void OnStartTransition()
        {
            if (gameObject.activeSelf)
                StopPatrol();
        }

        public void OnEndTransition()
        {
            //if (gameObject.activeSelf)
            //    StartPatrol();
        }

        private void StartPatrol()
        {
            Vector3 offset = ((Vector2)_targetPoint.position - (Vector2)_warningZoneTransform.position).normalized;
            _warningZoneTransform.rotation = Quaternion.LookRotation(Vector3.forward, offset);
            _coroutine = StartCoroutine(AwaitPatrol());
        }

        private void StopPatrol()
        {
            if (_coroutine != null)
                StopCoroutine(_coroutine);
        }

        private IEnumerator AwaitPatrol()
        {
            _warningZone.TriggerEnter += TriggerEnter;
            _guard.position = _startPoint.position;

            while (true)
            {
                yield return AwaitToMove(_targetPoint.position);
                yield return new WaitForSeconds(1f);
                yield return AwaitToMove(_startPoint.position);
                yield return new WaitForSeconds(1f);
            }
        }

        private void TriggerEnter(GameObject collisionObject)
        {
            if (collisionObject.TryGetComponent(out Player player))
            {
                _transitionService.Transition("MyCell", 0);
            }
        }

        private IEnumerator AwaitToMove(Vector2 target)
        {
            _spriteRenderer.flipX = _guard.position.x > target.x;
            
            //Vector2 direction = (target - (Vector2)transform.position).normalized;
            //float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            //_warningZoneTransform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
            
            Vector3 offset = (target - (Vector2)_warningZoneTransform.position).normalized;
            _warningZoneTransform.rotation = Quaternion.LookRotation(Vector3.forward, offset);
            
            _animator.SetBool("IsWalk", true);

            while (Vector2.Distance(target, _guard.position) != 0)
            {
                yield return null;
                _guard.position = Vector2.MoveTowards(_guard.position, target, Time.deltaTime);
            }
            
            _animator.SetBool("IsWalk", false);
        }
    }
}