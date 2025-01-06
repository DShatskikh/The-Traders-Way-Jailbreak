using System;
using System.Collections;
using DG.Tweening;
using PixelCrushers.DialogueSystem;
using UnityEngine;

namespace Game
{
    public class Skibidi : MonoBehaviour, IUseObject
    {
        [SerializeField]
        private Transform _upPoint, _downPoint, _head;

        [SerializeField]
        private DialogueSystemTrigger _dialogueSystemTrigger;

        [SerializeField]
        private DialogueSystemEvents _dialogueSystemEvents;
        
        private Sequence _animation;
        private GameStateController _gameStateController;
        private SaveData _saveData;
        
        [Serializable]
        public struct SaveData
        {
            public bool IsShow;
        }
        
        [Inject]
        private void Construct(GameStateController gameStateController)
        {
            _gameStateController = gameStateController;
        }
        
        public void Use()
        {
            if (_saveData.IsShow)
                _dialogueSystemTrigger.OnUse();
            else
                StartCoroutine(AwaitAnimation());
        }

        private IEnumerator AwaitAnimation()
        {
            _gameStateController.OpenDialog();
            
            _animation = DOTween.Sequence();
            yield return _animation.Append(_head.DOMoveY(_upPoint.position.y, 1f)).WaitForCompletion();
            yield return new WaitForSeconds(1);
            _dialogueSystemTrigger.OnUse();
            var isEnd = false;
            _dialogueSystemEvents.conversationEvents.onConversationEnd.AddListener((r) => isEnd = true);
            yield return new WaitUntil(() => isEnd);
            _dialogueSystemEvents.conversationEvents.onConversationEnd.RemoveAllListeners();

            _saveData.IsShow = true;
            CutscenesDataStorage.SetData("Skibidi", _saveData);
            yield return new WaitForSeconds(1);
            _gameStateController.OpenDialog();
            yield return _animation.Append(_head.DOMoveY(_downPoint.position.y, 1f)).WaitForCompletion();
            
            _gameStateController.CloseDialog();
            
            _dialogueSystemTrigger.OnUse();
        }
    }
}