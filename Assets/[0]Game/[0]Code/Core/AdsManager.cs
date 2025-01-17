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

        [Inject]
        private void Construct(HatManager hatManager, GameStateController gameStateController, WalletService walletService)
        {
            _hatManager = hatManager;
            _gameStateController = gameStateController;
            _walletService = walletService;
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
                    YandexMetrica.Send("Ads", new Dictionary<string, string>() { {"Ads", "FaceNoob"} });
                    break;
                case 1: // Деньги за рекламу
                    YandexMetrica.Send("Ads", new Dictionary<string, string>() { {"Ads", 
                        $"Money: {_walletService.GetFormatMoney(_walletService.GetMaxAward)}"} });
                    _walletService.Add(_walletService.GetMaxAward);
                    break;
                default:
                    break;
            }
        }
    }
}