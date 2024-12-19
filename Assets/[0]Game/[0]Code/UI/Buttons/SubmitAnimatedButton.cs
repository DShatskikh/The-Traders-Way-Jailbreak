using System.Collections;
using UnityEngine.InputSystem;

namespace Game
{
    public class SubmitAnimatedButton : AnimatedButton
    {
        private bool _isDown;
        private PlayerInput _playerInput;

        protected override void OnAwake()
        {
            base.OnAwake();
            _playerInput = ServiceLocator.Get<PlayerInput>();
        }

        protected override void Enable()
        {
            _playerInput.actions["Submit"].started += OnSubmitDown;
            _playerInput.actions["Submit"].performed += OnSubmitUp;

            //StartCoroutine(AwaitCheck());
        }

        protected override void Disable()
        {
            _playerInput.actions["Submit"].started -= OnSubmitDown;
            _playerInput.actions["Submit"].performed -= OnSubmitUp;
        }

        private void OnSubmitDown(InputAction.CallbackContext obj)
        {
            _isDown = true;
            _view.Down();
        }

        private void OnSubmitUp(InputAction.CallbackContext obj)
        {
            if (!_isDown)
                return;

            _isDown = false;
            _view.Up();
            _button.onClick.Invoke();
        }

        public override void OnClick()
        {
            if (!_isDown)
                return;
            
            Disable();
            //EventBus.SubmitUp?.Invoke();
        }
    }
}