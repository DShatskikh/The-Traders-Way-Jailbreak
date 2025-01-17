using System.Collections.Generic;
using UnityEngine;
using YG;

namespace Game
{
    public class EndingsGame
    {
        private GameStateController _gameStateController;
        private HatManager _hatManager;

        [Inject]
        private void Constructor(GameStateController gameStateController, HatManager hatManager)
        {
            _gameStateController = gameStateController;
            _hatManager = hatManager;
        }
        
        public void EndingGameStandard()
        {
            Debug.Log("EndGameStandard");
            YandexMetrica.Send("Ending", new Dictionary<string, string>() { {"Ending", "Standard"} });
            _gameStateController.OpenMainMenu();
        }
        
        public void EndingGameSecret()
        {
            Debug.Log("EndGameSecret");
            YandexMetrica.Send("Ending", new Dictionary<string, string>() { {"Ending", "Secret"} });
            _hatManager.BuyHat("HackerMask");
            _gameStateController.OpenMainMenu();
        }
    }
}