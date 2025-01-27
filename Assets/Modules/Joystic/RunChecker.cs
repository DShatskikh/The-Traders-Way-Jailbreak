using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.OnScreen;

namespace Game
{
    [AddComponentMenu("Input/On-Screen RunChecker")]
    [InputControlLayout]
    public class RunChecker : OnScreenControl
    {
        [InputControl(layout = "Button")]
        [SerializeField]
        private string m_ControlPath;
        
        //private InputJoystick _joystick;
        private PlayerInput _playerInput;
        
        protected override string controlPathInternal
        {
            get => m_ControlPath;
            set => m_ControlPath = value;
        }
    }
}