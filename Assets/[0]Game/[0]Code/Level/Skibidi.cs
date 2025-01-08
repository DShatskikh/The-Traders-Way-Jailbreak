using System;
using System.Collections;
using DG.Tweening;
using PixelCrushers.DialogueSystem;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game
{
    public class Skibidi : MonoBehaviour, IUseObject
    {
        [SerializeField]
        private Transform _upPoint, _downPoint, _head;

        [SerializeField]
        private DialogueSystemTrigger _showSkibidiDialog;

        [SerializeField]
        private DialogueSystemTrigger _defaultDialog;
        
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
            if (Random.Range(0, 10) == 6)
            {
                if (_saveData.IsShow)
                    _showSkibidiDialog.OnUse();
                else
                    StartCoroutine(AwaitAnimation());
            }
            else
            {
                _defaultDialog.OnUse();
            }
        }

        private IEnumerator AwaitAnimation()
        {
            _gameStateController.OpenDialog();
            
            _animation = DOTween.Sequence();
            yield return _animation.Append(_head.DOMoveY(_upPoint.position.y, 1f)).WaitForCompletion();
            yield return new WaitForSeconds(1);
            
            _showSkibidiDialog.OnUse();
            yield return DialogueExtensions.AwaitCloseDialog();

            _saveData.IsShow = true;
            CutscenesDataStorage.SetData("Skibidi", _saveData);
            yield return new WaitForSeconds(1);
            _gameStateController.OpenDialog();
            yield return _animation.Append(_head.DOMoveY(_downPoint.position.y, 1f)).WaitForCompletion();
            _gameStateController.CloseDialog();
            _showSkibidiDialog.OnUse();
        }
    }
}