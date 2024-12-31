using System;
using UnityEngine;

namespace Game
{
    public class TransitionDialogueState : MonoBehaviour
    {
        private GameStateController _gameStateController;
        private bool _isInit;

        private void Awake()
        {
            _gameStateController = ServiceLocator.Get<GameStateController>();
        }

        public void OpenDialogueState()
        {
            if (_isInit)
                _gameStateController.OpenDialog();
            else
                _isInit = true;
        }
        
        public void CloseDialogueState()
        {
            _gameStateController.CloseDialog();
        }
    }
}