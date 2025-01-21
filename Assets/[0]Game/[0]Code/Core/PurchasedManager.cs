using System;
using UnityEngine;
using YG;

namespace Game
{
    [Serializable]
    public class PurchasedManager
    {
        private HatManager _hatManager;
        private GameStateController _gameStateController;
        private IAnalyticsService _analyticsService;
        private ISaveLoadService _saveLoadService;

        [Inject]
        private void Construct(HatManager hatManager, GameStateController gameStateController, 
            IAnalyticsService analyticsService, ISaveLoadService saveLoadService)
        {
            _hatManager = hatManager;
            _gameStateController = gameStateController;
            _analyticsService = analyticsService;
            _saveLoadService = saveLoadService;
            
            YandexGame.PurchaseSuccessEvent += PurchaseSuccessEvent;
        }

        ~PurchasedManager()
        {
            YandexGame.PurchaseSuccessEvent -= PurchaseSuccessEvent;
        }

        private void PurchaseSuccessEvent(string obj)
        {
            Debug.Log($"Buy: {obj}");

            switch (obj)
            {
                case "Herobrine":
                    _hatManager.BuyHat("Herobrine");
                    _gameStateController.OpenShop();
                    _analyticsService.Send("Purchased", "Herobrine");
                    break;
                default:
                    break;
            }
            
            _saveLoadService.Save();
        }
    }
}