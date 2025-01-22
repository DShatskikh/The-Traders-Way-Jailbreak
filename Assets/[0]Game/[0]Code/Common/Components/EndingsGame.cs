using UnityEngine;

namespace Game
{
    public class EndingsGame
    {
        private GameStateController _gameStateController;
        private HatManager _hatManager;
        private IAnalyticsService _analyticsService;
        private ISaveLoadService _saveLoadService;

        [Inject]
        private void Constructor(GameStateController gameStateController, HatManager hatManager, 
            IAnalyticsService analyticsService, ISaveLoadService saveLoadService)
        {
            _gameStateController = gameStateController;
            _hatManager = hatManager;
            _analyticsService = analyticsService;
            _saveLoadService = saveLoadService;
        }
        
        public void EndingGameStandard()
        {
            var data = RepositoryStorage.Get<EndsData>(KeyConstants.Ending);
            data.IsDefaultEnding = true;
            RepositoryStorage.Set(KeyConstants.Ending, data);

            _saveLoadService.Reset();
            
            Debug.Log("EndGameStandard");
            _analyticsService.Send("Ending", "Standard");
            _gameStateController.OpenMainMenu();
            _saveLoadService.Save();
        }
        
        public void EndingGameSecret()
        {
            var data = RepositoryStorage.Get<EndsData>(KeyConstants.Ending);
            data.IsSecretEnding = true;
            RepositoryStorage.Set(KeyConstants.Ending, data);
            
            _saveLoadService.Reset();
            
            Debug.Log("EndGameSecret");
            _analyticsService.Send("Ending", "Secret");
            _hatManager.BuyHat("HackerMask");
            _gameStateController.OpenMainMenu();
            _saveLoadService.Save();
        }
    }
}