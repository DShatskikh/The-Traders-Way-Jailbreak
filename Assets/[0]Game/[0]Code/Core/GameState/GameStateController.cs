using System.Linq;
using UnityEngine;

namespace Game
{
    public class GameStateController : MonoBehaviour
    {
        [SerializeField]
        private Transform _level;

        [SerializeField]
        private Transform _screens;
        
        private ScreenManager _screenManager;
        private GameState _state;

        private IResumeGame[] _resumeGames;
        private IPausedGame[] _pausedGames;
        
        private IUpdate[] _updates;
        private IFixedUpdate[] _fixedUpdates;
        
        private void Awake()
        {
            _screenManager = ServiceLocator.Get<ScreenManager>();
            
            _resumeGames = _level.GetComponentsInChildren<IResumeGame>();
            var rg = _resumeGames.ToList();
            rg.AddRange(_screens.GetComponentsInChildren<IResumeGame>());
            _resumeGames = rg.ToArray();
            
            _pausedGames = _level.GetComponentsInChildren<IPausedGame>();
            var pg = _pausedGames.ToList();
            pg.AddRange(_screens.GetComponentsInChildren<IPausedGame>());
            _pausedGames = pg.ToArray();
            
            _updates = _level.GetComponentsInChildren<IUpdate>();
            _fixedUpdates = _level.GetComponentsInChildren<IFixedUpdate>();
        }

        private void Update()
        {
            for (int i = 0; i < _updates.Length; i++) 
                _updates[i].OnUpdate();
        }

        private void FixedUpdate()
        {
            for (int i = 0; i < _fixedUpdates.Length; i++) 
                _fixedUpdates[i].OnFixedUpdate();
        }

        public void StartGame()
        {
            _state = GameState.Playing;
            
            foreach (var resume in _resumeGames) 
                resume.ResumeGame();
        }
        
        public void PauseGame()
        {
            _state = GameState.Paused;

            foreach (var paused in _pausedGames) 
                paused.PausedGame();

            _screenManager.Show(ScreenType.Pause);
            Time.timeScale= 0;
        }
        
        public void ResumeGame()
        {
            if (_state != GameState.Paused)
                return;
            
            _state = GameState.Playing;
            
            foreach (var resume in _resumeGames) 
                resume.ResumeGame();
            
            _screenManager.Hide(ScreenType.Pause);
            Time.timeScale= 1;
        }
        
        public void OpenLaptop()
        {
            _state = GameState.Paused;

            foreach (var paused in _pausedGames) 
                paused.PausedGame();

            _screenManager.Show(ScreenType.Laptop);
        }

        public void CloseLaptop()
        {
            _state = GameState.Playing;

            foreach (var resume in _resumeGames) 
                resume.ResumeGame();

            _screenManager.Hide(ScreenType.Laptop);
        }
        
        public void FinishGame()
        {
            _state = GameState.Paused;
        }
    }
}