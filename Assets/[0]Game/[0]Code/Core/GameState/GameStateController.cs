using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class GameStateController : MonoBehaviour
    {
        private enum GameState : byte
        {
            OFF = 0,
            PLAYING = 1,
            PAUSED = 2,
            FINISHED = 3,
            TRANSITION = 4,
            LAPTOP = 5,
            SHOP = 6,
            ADS = 7,
            DIALOGUE = 8,
        }

        private GameState _gameState;
        private readonly List<IGameListener> _listeners = new();
        private readonly List<IGameUpdateListener> _updateListeners = new();
        private readonly List<IGameFixedUpdateListener> _fixedUpdateListeners = new();

        private void Update()
        {
            if (_gameState != GameState.PLAYING)
                return;

            for (int i = 0; i < _updateListeners.Count; i++) 
                _updateListeners[i].OnUpdate();
        }
        
        private void FixedUpdate()
        {
            if (_gameState != GameState.PLAYING)
                return;

            for (int i = 0; i < _fixedUpdateListeners.Count; i++) 
                _fixedUpdateListeners[i].OnFixedUpdate();
        }
        
        public void AddListener(IGameListener listener) 
        {
            if (listener == null)
                return;

            if (listener is IGameUpdateListener updateListener) 
                _updateListeners.Add(updateListener);

            if (listener is IGameFixedUpdateListener fixedUpdateListener)
                _fixedUpdateListeners.Add(fixedUpdateListener);

            _listeners.Add(listener);
        }

        public void StartGame() 
        {
            if (_gameState != GameState.OFF && _gameState != GameState.FINISHED)
                return;
            
            foreach (var listener in _listeners) 
            {
                if (listener is IGameStartListener startListeners) 
                    startListeners.OnStartGame();
            }
            
            _gameState = GameState.PLAYING;
        }

        public void PauseGame()
        {
            if (_gameState != GameState.PLAYING)
                return;
            
            foreach (var listener in _listeners)
            {
                if (listener is IGamePauseListener pauseListeners) 
                    pauseListeners.OnPauseGame();
            }

            _gameState = GameState.PAUSED;
        }

        public void ResumeGame()
        {
            if (_gameState != GameState.PAUSED)
                return;
            
            foreach (var listener in _listeners)
            {
                if (listener is IGameResumeListener resumeListeners) 
                    resumeListeners.OnResumeGame();
            }
            
            _gameState = GameState.PLAYING;
        }

        public void StartTransition()
        {
            if (_gameState != GameState.PLAYING)
                return;
            
            foreach (var listener in _listeners)
            {
                if (listener is IGameTransitionListener transitionListener) 
                    transitionListener.OnStartTransition();
            }
            
            _gameState = GameState.TRANSITION;
        }

        public void EndTransition()
        {
            if (_gameState != GameState.TRANSITION)
                return;
            
            foreach (var listener in _listeners)
            {
                if (listener is IGameTransitionListener transitionListener) 
                    transitionListener.OnEndTransition();
            }
            
            _gameState = GameState.PLAYING;
        }
        
        public void OpenLaptop()
        {
            if (_gameState != GameState.PLAYING)
                return;
            
            foreach (var listener in _listeners)
            {
                if (listener is IGameLaptopListener laptopListener) 
                    laptopListener.OnOpenLaptop();
            }
            
            _gameState = GameState.LAPTOP;
        }

        public void CloseLaptop()
        {
            if (_gameState != GameState.LAPTOP)
                return;
            
            foreach (var listener in _listeners)
            {
                if (listener is IGameLaptopListener laptopListener) 
                    laptopListener.OnCloseLaptop();
            }
            
            _gameState = GameState.PLAYING;
        }
        
        public void OpenShop()
        {
            if (_gameState != GameState.PLAYING)
                return;
            
            foreach (var listener in _listeners)
            {
                if (listener is IGameShopListener shopListener) 
                    shopListener.OnOpenShop();
            }
            
            _gameState = GameState.SHOP;
        }

        public void CloseShop()
        {
            if (_gameState != GameState.SHOP)
                return;
            
            foreach (var listener in _listeners)
            {
                if (listener is IGameShopListener shopListener) 
                    shopListener.OnCloseShop();
            }
            
            _gameState = GameState.PLAYING;
        }
        
        public void OpenADS()
        {
            if (_gameState != GameState.PLAYING)
                return;
            
            foreach (var listener in _listeners)
            {
                if (listener is IGameADSListener adsListener) 
                    adsListener.OnShowADS();
            }
            
            _gameState = GameState.ADS;
        }

        public void CloseADS()
        {
            if (_gameState != GameState.ADS)
                return;
            
            foreach (var listener in _listeners)
            {
                if (listener is IGameADSListener adsListener) 
                    adsListener.OnHideADS();
            }
            
            _gameState = GameState.PLAYING;
        }

        public void OpenDialog()
        {
            if (_gameState != GameState.PLAYING)
                return;
            
            foreach (var listener in _listeners)
            {
                if (listener is IGameDialogueListener dialogueListener) 
                    dialogueListener.OnShowDialogue();
            }
            
            _gameState = GameState.DIALOGUE;
        }

        public void CloseDialog()
        {
            if (_gameState != GameState.DIALOGUE)
                return;
            
            foreach (var listener in _listeners)
            {
                if (listener is IGameDialogueListener dialogueListener) 
                    dialogueListener.OnHideDialogue();
            }
            
            _gameState = GameState.PLAYING;
        }
    }
}