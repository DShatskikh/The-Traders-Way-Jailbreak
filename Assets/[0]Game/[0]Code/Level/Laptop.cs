using TMPro;
using UnityEngine;
using UnityEngine.Localization;

namespace Game
{
    public sealed class Laptop : MonoBehaviour, IUseObject
    {
        [SerializeField]
        private LocalizedString _localizedString;

        [SerializeField]
        private TMP_Text _label;

        private GameStateController _gameStateController;

        [Inject]
        private void Construct(GameStateController gameStateController)
        {
            _gameStateController = gameStateController;
        }

        private void Start()
        {
            LocalizedTextUtility.Load(_localizedString, (result) => _label.text = result);
        }

        public void Use()
        {
            _gameStateController.OpenLaptop();
        }
    }
}