using UnityEngine;

namespace Game
{
    public class Laptop : MonoBehaviour, IUseObject
    {
        private GameStateController _gameStateController;
        
        private void Awake()
        {
            _gameStateController = ServiceLocator.Get<GameStateController>();
        }

        public void Use()
        {
            _gameStateController.OpenLaptop();
        }
    }
}