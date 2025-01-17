using System;
using System.Collections;
using PixelCrushers.DialogueSystem;
using UnityEngine;

namespace Game
{
    public class NoobikSkinShop : MonoBehaviour, IUseObject, IGameShopListener
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
        
        [Inject]
        private void Construct(GameStateController gameStateController)
        {
            _gameStateController = gameStateController;
        }
        
        public void Use()
        {
            if (_saveData.IsNotFirstOpen)
                _gameStateController.OpenShop();
            else
            {
                _dialogueSystemTrigger.OnUse();
                _saveData.IsNotFirstOpen = true;
                RepositoryStorage.Set(KeyConstants.SkinShop, _saveData);

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
            StartCoroutine(AwaitCloseShop());
        }

        private IEnumerator AwaitCloseShop()
        {
            yield return new WaitForSeconds(1);
            
            if (_gameStateController.CurrentState == GameStateController.GameState.PLAYING)
                _dialogueSystemTrigger.OnUse();
        }
    }
}