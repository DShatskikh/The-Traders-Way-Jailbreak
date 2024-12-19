using UnityEngine.InputSystem;

namespace Game
{
    public class CancelAnimatedButton : AnimatedButton
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
            _playerInput.actions["Cancel"].started += OnSubmitDown;
            _playerInput.actions["Cancel"].performed += OnSubmitUp;
        }

        protected override void Disable()
        {
            _playerInput.actions["Cancel"].started -= OnSubmitDown;
            _playerInput.actions["Cancel"].performed -= OnSubmitUp;
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
            Disable();
            //_playerInput.actions["Cancel"]EventBus.CancelUp?.Invoke();
        }
    }
}