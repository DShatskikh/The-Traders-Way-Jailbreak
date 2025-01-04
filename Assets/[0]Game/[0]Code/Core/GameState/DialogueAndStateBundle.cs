using System;
using UnityEngine;

namespace Game
{
    public class DialogueAndStateBundle : MonoBehaviour
    {
        private GameStateController _gameStateController;

        [Inject]
        private void Construct(GameStateController gameStateController)
        {
            _gameStateController = gameStateController;
        }

        public void OpenDialogue()
        {
            _gameStateController.OpenDialog();
        }
        
        public void CloseDialogue()
        {
            _gameStateController.CloseDialog();
        }
    }
}