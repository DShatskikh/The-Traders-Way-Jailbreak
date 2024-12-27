using UnityEngine;

namespace Game
{
    public class SkinShop : MonoBehaviour, IUseObject
    {
        private GameStateController _gameStateController;
        
        private void Awake()
        {
            _gameStateController = ServiceLocator.Get<GameStateController>();
        }

        public void Use()
        {
            _gameStateController.OpenShop();
        }
    }
}