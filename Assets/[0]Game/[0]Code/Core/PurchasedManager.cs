using System;
using System.Collections.Generic;
using UnityEngine;
using YG;

namespace Game
{
    [Serializable]
    public class PurchasedManager
    {
        private HatManager _hatManager;
        private GameStateController _gameStateController;

        [Inject]
        private void Construct(HatManager hatManager, GameStateController gameStateController)
        {
            _hatManager = hatManager;
            _gameStateController = gameStateController;
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
                    YandexMetrica.Send("Buy", new Dictionary<string, string>() { {"Buy", "Herobrine"} });
                    break;
                default:
                    break;
            }
        }
    }
}