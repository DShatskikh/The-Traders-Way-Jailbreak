using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class GameStateController : MonoBehaviour
    {
        public enum GameState : byte
        {
            OFF = 0,
            PLAYING = 1,
            PAUSED = 2,
            MAIN_MENU = 3,
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

        public GameState CurrentState => _gameState;
        
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

        public void RemoveListener(IGameListener listener)
        {
            if (listener is IGameUpdateListener updateListener) 
                _updateListeners.Remove(updateListener);

            if (listener is IGameFixedUpdateListener fixedUpdateListener)
                _fixedUpdateListeners.Remove(fixedUpdateListener);

            _listeners.Remove(listener);
        }

        public void OpenMainMenu()
        {
            _gameState = GameState.MAIN_MENU;
            
            foreach (var listener in _listeners) 
            {
                if (listener is IGameMainMenuListener startListeners) 
                    startListeners.OnOpenMainMenu();
            }
        }
        
        public void StartGame() 
        {
            _gameState = GameState.PLAYING;
            
            foreach (var listener in _listeners) 
            {
                if (listener is IGameStartListener startListeners) 
                    startListeners.OnStartGame();
            }
        }

        public void PauseGame()
        {
            if (_gameState != GameState.PLAYING)
                return;

            _gameState = GameState.PAUSED;
            
            foreach (var listener in _listeners)
            {
                if (listener is IGamePauseListener pauseListeners) 
                    pauseListeners.OnPauseGame();
            }
        }

        public void ResumeGame()
        {
            if (_gameState != GameState.PAUSED)
                return;

            _gameState = GameState.PLAYING;
            
            foreach (var listener in _listeners)
            {
                if (listener is IGameResumeListener resumeListeners) 
                    resumeListeners.OnResumeGame();
            }
        }

        public void StartTransition()
        {
            if (_gameState != GameState.PLAYING)
                return;

            _gameState = GameState.TRANSITION;
            
            foreach (var listener in _listeners)
            {
                if (listener is IGameTransitionListener transitionListener) 
                    transitionListener.OnStartTransition();
            }
        }

        public void EndTransition()
        {
            if (_gameState != GameState.TRANSITION)
                return;

            _gameState = GameState.PLAYING;
            
            foreach (var listener in _listeners)
            {
                if (listener is IGameTransitionListener transitionListener) 
                    transitionListener.OnEndTransition();
            }
        }
        
        public void OpenLaptop()
        {
            if (_gameState != GameState.PLAYING)
                return;

            _gameState = GameState.LAPTOP;
            
            foreach (var listener in _listeners)
            {
                if (listener is IGameLaptopListener laptopListener) 
                    laptopListener.OnOpenLaptop();
            }
        }

        public void CloseLaptop()
        {
            if (_gameState != GameState.LAPTOP)
                return;

            _gameState = GameState.PLAYING;
            
            foreach (var listener in _listeners)
            {
                if (listener is IGameLaptopListener laptopListener) 
                    laptopListener.OnCloseLaptop();
            }
        }
        
        public void OpenShop()
        {
            if (_gameState != GameState.PLAYING)
                return;

            _gameState = GameState.SHOP;
            
            foreach (var listener in _listeners)
            {
                if (listener is IGameShopListener shopListener) 
                    shopListener.OnOpenShop();
            }
        }

        public void CloseShop()
        {
            if (_gameState != GameState.SHOP)
                return;

            _gameState = GameState.PLAYING;
            
            foreach (var listener in _listeners)
            {
                if (listener is IGameShopListener shopListener) 
                    shopListener.OnCloseShop();
            }
        }
        
        public void OpenADS()
        {
            if (_gameState != GameState.PLAYING)
                return;

            _gameState = GameState.ADS;
            
            foreach (var listener in _listeners)
            {
                if (listener is IGameADSListener adsListener) 
                    adsListener.OnShowADS();
            }
        }

        public void CloseADS()
        {
            if (_gameState != GameState.ADS)
                return;

            _gameState = GameState.PLAYING;
            
            foreach (var listener in _listeners)
            {
                if (listener is IGameADSListener adsListener) 
                    adsListener.OnHideADS();
            }
        }

        public void OpenDialog()
        {
            _gameState = GameState.DIALOGUE;
            
            foreach (var listener in _listeners)
            {
                if (listener is IGameDialogueListener dialogueListener) 
                    dialogueListener.OnShowDialogue();
            }
        }

        public void CloseDialog()
        {
            if (_gameState != GameState.DIALOGUE)
                return;

            _gameState = GameState.PLAYING;
            
            foreach (var listener in _listeners)
            {
                if (listener is IGameDialogueListener dialogueListener) 
                    dialogueListener.OnHideDialogue();
            }
        }
    }
}