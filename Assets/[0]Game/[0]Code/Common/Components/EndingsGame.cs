using UnityEngine;

namespace Game
{
    public class EndingsGame
    {
        private GameStateController _gameStateController;
        private HatManager _hatManager;
        private IAnalyticsService _analyticsService;

        [Inject]
        private void Constructor(GameStateController gameStateController, HatManager hatManager, IAnalyticsService analyticsService)
        {
            _gameStateController = gameStateController;
            _hatManager = hatManager;
            _analyticsService = analyticsService;
        }
        
        public void EndingGameStandard()
        {
            var data = RepositoryStorage.Get<EndsData>(KeyConstants.Ending);
            data.IsDefaultEnding = true;
            RepositoryStorage.Set(KeyConstants.Ending, data);
            
            Debug.Log("EndGameStandard");
            _analyticsService.Send("Ending", "Standard");
            _gameStateController.OpenMainMenu();
        }
        
        public void EndingGameSecret()
        {
            var data = RepositoryStorage.Get<EndsData>(KeyConstants.Ending);
            data.IsSecretEnding = true;
            RepositoryStorage.Set(KeyConstants.Ending, data);
            
            Debug.Log("EndGameSecret");
            _analyticsService.Send("Ending", "Secret");
            _hatManager.BuyHat("HackerMask");
            _gameStateController.OpenMainMenu();
        }
    }
}