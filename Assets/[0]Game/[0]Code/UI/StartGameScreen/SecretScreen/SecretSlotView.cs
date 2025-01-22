using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game
{
    public sealed class SecretSlotView : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField]
        private Image _icon;
        
        [SerializeField]
        private TMP_Text _label;

        public event Action PointerDown;
        public event Action PointerUp;

        public void OnPointerDown(PointerEventData eventData) => 
            PointerDown?.Invoke();

        public void OnPointerUp(PointerEventData eventData) => 
            PointerUp?.Invoke();

        public void SetIcon(Sprite icon)
        {
            _icon.sprite = icon;
        }

        public void SetLabel(string text)
        {
            _label.text = text;
        }

        public void SetLabelColor(Color color)
        {
            _label.color = color;
        }
    }
}