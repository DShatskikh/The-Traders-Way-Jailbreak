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

        [Inject]
        private void Construct(GameStateController gameStateController)
        {
            _gameStateController = gameStateController;
        }
        
        public void Use()
        {
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
            
            yield return new WaitForSeconds(1);
            yield return _animation.Append(_head.DOMoveY(_downPoint.position.y, 1f)).WaitForCompletion();
            
            _gameStateController.CloseDialog();
        }
    }
}