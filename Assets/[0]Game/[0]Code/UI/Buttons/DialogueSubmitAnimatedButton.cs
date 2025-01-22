using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game
{
    public sealed class DialogueSubmitAnimatedButton : MonoBehaviour
    {
        private PlayerInput _playerInput;
        private AnimatedButton _animatedButton;

        private void Awake()
        {
            _playerInput = ServiceLocator.Get<PlayerInput>();
            _animatedButton = GetComponent<AnimatedButton>();
        }

        private void OnEnable()
        {
            _playerInput.actions["Submit"].started += OnSubmitDown;
            _playerInput.actions["Submit"].canceled += OnSubmitUp;
        }

        private void OnDisable()
        {
            if (_playerInput)
            {
                _playerInput.actions["Submit"].started -= OnSubmitDown;
                _playerInput.actions["Submit"].canceled -= OnSubmitUp;
                
                _playerInput.actions["Submit"].started -= OnSubmitDown;
                _playerInput.actions["Submit"].canceled -= OnSubmitUp;
            }

            if (CoroutineRunner.Instance)
                CoroutineRunner.Instance.StartCoroutine(AwaitShow());
        }

        private IEnumerator AwaitShow()
        {
            yield return null;
            gameObject.SetActive(true);
        }

        private void OnSubmitDown(InputAction.CallbackContext obj)
        {
            _animatedButton.OnPointerDown(null);
        }

        private void OnSubmitUp(InputAction.CallbackContext obj)
        {
            _animatedButton.OnPointerUp(null);
            _animatedButton.onClick?.Invoke();
        }
    }
}