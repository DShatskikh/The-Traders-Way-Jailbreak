namespace Game
{
    public class LocationLoader
    {
        private LocationsManager _locationsManager;
        private GameStateController _gameStateController;
        private ScreenManager _screenManager;
        private Player _player;

        [Inject]
        private void Construct(LocationsManager locationsManager, GameStateController gameStateController, 
            ScreenManager screenManager, Player player)
        {
            _locationsManager = locationsManager;
            _gameStateController = gameStateController;
            _screenManager = screenManager;
            _player = player;
        }

        public void Load()
        {
            _locationsManager.SwitchLocation("World", 0);
            
            if (_gameStateController.CurrentState == GameStateController.GameState.PAUSED)
                _gameStateController.ResumeGame();
            else if (_gameStateController.CurrentState == GameStateController.GameState.DIALOGUE)
                _gameStateController.CloseDialog();
            
            _screenManager.Show(ScreenType.Main);
            _player.gameObject.SetActive(true);
        }
    }
}