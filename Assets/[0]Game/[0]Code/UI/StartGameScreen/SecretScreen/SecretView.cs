using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public sealed class SecretView : MonoBehaviour
    {
        [SerializeField]
        private Button _exitButton;

        [SerializeField]
        private Transform _container;

        [SerializeField]
        private Image _icon;

        [SerializeField]
        private TMP_Text _description;
        
        public Button ExitButton => _exitButton;
        public Transform Container => _container;

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void SetIcon(Sprite icon)
        {
            _icon.sprite = icon;
        }
        
        public void SetDescription(string description)
        {
            _description.text = description;
        }
    }
}