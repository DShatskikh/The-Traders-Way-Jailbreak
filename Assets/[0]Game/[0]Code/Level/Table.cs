using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;

namespace Game
{
    public class Table : MonoBehaviour, IUseObject
    {
        [Header("Data")]
        [SerializeField]
        private LocalizedString _text;
        
        [Header("Links")]
        [SerializeField]
        private GameObject _hud;

        [SerializeField]
        private TMP_Text _label;
        
        [SerializeField]
        private Button _button;

        private GameStateController _gameStateController;

        [Inject]
        private void Construct(GameStateController gameStateController)
        {
            _gameStateController = gameStateController;
        }
        
        private void OnEnable()
        {
            _button.onClick.AddListener(Close);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(Close);
        }

        public void Use()
        {
            _gameStateController.OpenDialog();
            LocalizedTextUtility.Load(_text, text =>
            {
                _label.text = text;
                _hud.SetActive(true);
            });
        }

        private void Close()
        {
            _hud.SetActive(false);
            _gameStateController.CloseDialog();
        }
    }
}