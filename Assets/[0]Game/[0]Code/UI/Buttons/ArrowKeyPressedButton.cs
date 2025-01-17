using UnityEngine;
using UnityEngine.InputSystem;

namespace Game
{
    public class ArrowKeyPressedButton : MonoBehaviour
    {
        [SerializeField]
        private InputActionReference m_Action;

        [SerializeField]
        private Vector2 _direction;
        
        private AnimatedButton _animatedButton;
        private bool _isClick;

        private void Awake()
        {
            _animatedButton = GetComponent<AnimatedButton>();
        }

        private void OnEnable()
        {
            var action = m_Action.action;
            action.started += ActionOnstarted;
            action.canceled += ActionOncanceled;
        }

        private void OnDisable()
        {
            var action = m_Action.action;
            action.started -= ActionOnstarted;
            action.canceled -= ActionOncanceled;
        }

        private void ActionOnstarted(InputAction.CallbackContext obj)
        {
            if (_direction == obj.ReadValue<Vector2>().normalized)
            {
                _animatedButton.OnPointerDown(null);
                _isClick = true;
            }
        }

        private void ActionOncanceled(InputAction.CallbackContext obj)
        {
            if (_isClick)
            {
                _isClick = false;
                _animatedButton.OnPointerUp(null);
                _animatedButton.onClick.Invoke(); 
            }
        }
    }
}