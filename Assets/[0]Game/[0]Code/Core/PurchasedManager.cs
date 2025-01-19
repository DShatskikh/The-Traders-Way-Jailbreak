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
        
        [Inject]
        private void Construct(HatManager hatManager, GameStateController gameStateController, IAnalyticsService analyticsService)
        {
            _hatManager = hatManager;
            _gameStateController = gameStateController;
            _analyticsService = analyticsService;
            
            
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
        }
    }
}