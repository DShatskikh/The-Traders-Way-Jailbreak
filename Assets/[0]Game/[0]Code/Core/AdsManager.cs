using System;
using System.Collections.Generic;
using YG;

namespace Game
{
    [Serializable]
    public class AdsManager
    {
        private HatManager _hatManager;
        private GameStateController _gameStateController;
        private WalletService _walletService;
        private IAnalyticsService _analyticsService;

        [Inject]
        private void Construct(HatManager hatManager, GameStateController gameStateController, WalletService walletService, IAnalyticsService analyticsService)
        {
            _hatManager = hatManager;
            _gameStateController = gameStateController;
            _walletService = walletService;
            _analyticsService = analyticsService;
            
            YandexGame.RewardVideoEvent += RewardVideoEvent;
        }

        ~AdsManager()
        {
            YandexGame.RewardVideoEvent -= RewardVideoEvent;
        }

        private void RewardVideoEvent(int index)
        {
            switch (index)
            {
                case 0: // Лицо нубика
                    _hatManager.BuyHat("FaceNoob");
                    _gameStateController.OpenShop();
                    _analyticsService.Send("Ads", "FaceNoob");
                    break;
                case 1: // Деньги за рекламу
                    _walletService.Add(_walletService.GetMaxAward);
                    _analyticsService.Send("Ads",  $"Money: {_walletService.GetFormatMoney(_walletService.GetMaxAward)}");
                    break;
                default:
                    break;
            }
        }
    }
}