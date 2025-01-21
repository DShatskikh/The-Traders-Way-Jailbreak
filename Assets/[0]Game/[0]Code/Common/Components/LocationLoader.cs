namespace Game
{
    public class LocationLoader
    {
        private LocationsManager _locationsManager;
        private GameStateController _gameStateController;
        private ScreenManager _screenManager;
        private Player _player;
        private CoroutineRunner _coroutineRunner;

        [Inject]
        private void Construct(LocationsManager locationsManager, GameStateController gameStateController, 
            ScreenManager screenManager, Player player, CoroutineRunner coroutineRunner)
        {
            _locationsManager = locationsManager;
            _gameStateController = gameStateController;
            _screenManager = screenManager;
            _player = player;
            _coroutineRunner = coroutineRunner;
        }

        public void Load()
        {
            _gameStateController.StartGame();
            _screenManager.Show(ScreenType.Main);
            _player.gameObject.SetActive(true);

            var locationData = RepositoryStorage.Get<LocationsManager.Data>(KeyConstants.Location);

            if (locationData.LocationName == "") 
                locationData.LocationName = "World";

            _locationsManager.SwitchLocation(locationData.LocationName, 0);
        }
    }
}