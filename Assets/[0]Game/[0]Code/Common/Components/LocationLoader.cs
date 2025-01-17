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
            _gameStateController.StartGame();
            _screenManager.Show(ScreenType.Main);
            _player.gameObject.SetActive(true);
        }
    }
}