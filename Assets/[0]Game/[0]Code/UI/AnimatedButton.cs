using MoreMountains.Feedbacks;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Game
{
    public class AnimatedButton : Button
    {
        [SerializeField]
        protected Color _pressedColor;
        
        [SerializeField]
        protected Color _notPressedColor;
        
        [SerializeField]
        private TMP_Text _label;

        [SerializeField]
        private Image _frame;
        
        [SerializeField]
        private MMF_Player _enterAnimation;
        
        [SerializeField]
        private MMF_Player _exitAnimation;

        private bool _isPress;
        
        public override void OnPointerDown(PointerEventData eventData)
        {
            if (_isPress)
                return;
            
            if (eventData != null)
                base.OnPointerDown(eventData);
            
            StartClick();
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            if (!_isPress)
                return;
            
            if (eventData != null)
                base.OnPointerUp(eventData);

            EndClick();
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            NotGuided();
            base.OnPointerClick(eventData);
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);
            Guided();
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);
            NotGuided();
        }

        public override void OnSelect(BaseEventData eventData)
        {
            base.OnSelect(eventData);
            Guided();
        }

        public override void OnDeselect(BaseEventData eventData)
        {
            base.OnDeselect(eventData);
            NotGuided();
        }

        public override void OnSubmit(BaseEventData eventData)
        {
            StartClick();
        }

        protected virtual void Guided()
        {
            _label.color = _pressedColor;
            _frame.color = _pressedColor;
        }
        
        protected virtual void NotGuided()
        {
            _label.color = _notPressedColor;
            _frame.color = _notPressedColor;
        }
        
        private void StartClick()
        {
            _isPress = true;

            Guided();
            _enterAnimation.PlayFeedbacks();
        }
        
        private void EndClick()
        {
            if (!_isPress)
                return;
            
            _isPress = false;
            
            NotGuided();
            _exitAnimation.PlayFeedbacks();
            
            SoundPlayer.Play(AssetProvider.Instance.ClickSound);
        }
    }
}