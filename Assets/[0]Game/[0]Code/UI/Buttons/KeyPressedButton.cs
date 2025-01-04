using UnityEngine;
using UnityEngine.InputSystem;

namespace Game
{
    public class KeyPressedButton : MonoBehaviour
    {
        [SerializeField]
        private InputActionReference m_Action;
        
        private AnimatedButton _animatedButton;

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
            _animatedButton.OnPointerDown(null);
            print("ActionOnstarted");
        }

        private void ActionOncanceled(InputAction.CallbackContext obj)
        {
            _animatedButton.OnPointerUp(null);
            _animatedButton.onClick.Invoke();
            print("ActionOncanceled");
        }
    }
}