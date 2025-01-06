using System;
using PixelCrushers.DialogueSystem;
using UnityEngine;

namespace Game
{
    public class SkinShop : MonoBehaviour, IUseObject, IGameShopListener
    {
        [SerializeField]
        private DialogueSystemTrigger _dialogueSystemTrigger;
        
        private GameStateController _gameStateController;
        private SaveData _saveData;
        
        [Serializable]
        public struct SaveData
        {
            public bool IsNotFirstOpen;
        }
        
        private void Awake()
        {
            _gameStateController = ServiceLocator.Get<GameStateController>();
        }

        public void Use()
        {
            if (_saveData.IsNotFirstOpen)
                _gameStateController.OpenShop();
            else
            {
                _dialogueSystemTrigger.OnUse();
                _saveData.IsNotFirstOpen = true;
                CutscenesDataStorage.SetData("SkinShop", _saveData);

                DialogueExtensions.SubscriptionCloseDialog(() =>
                {
                    _gameStateController.OpenShop();
                });
            }
        }

        public void OnOpenShop()
        {
            
        }

        public void OnCloseShop()
        {
            _dialogueSystemTrigger.OnUse();
        }
    }
}