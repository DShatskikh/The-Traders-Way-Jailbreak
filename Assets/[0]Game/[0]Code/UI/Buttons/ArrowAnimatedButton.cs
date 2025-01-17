using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class ArrowAnimatedButton : AnimatedButton
    {
        [SerializeField]
        private Image _arrow;

        protected override void Guided()
        {
            base.Guided();
            _arrow.color = _pressedColor;
        }

        protected override void NotGuided()
        {
            base.NotGuided();
            _arrow.color = _notPressedColor;
        }
    }
}