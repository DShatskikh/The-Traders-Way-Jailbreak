using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class ShopScreen : ScreenBase, IGameShopListener
    {
        [SerializeField]
        private Button _exitButton;

        private GameStateController _gameStateController;

        [Inject]
        private void Construct(GameStateController gameStateController)
        {
            _gameStateController = gameStateController;
        }
        
        private void OnEnable()
        {
            _exitButton.onClick.AddListener(_gameStateController.CloseShop);
        }

        private void OnDisable()
        {
            _exitButton.onClick.RemoveAllListeners();
        }

        public void OnOpenShop()
        {
            Show();
        }

        public void OnCloseShop()
        {
            Hide();
        }
    }
}