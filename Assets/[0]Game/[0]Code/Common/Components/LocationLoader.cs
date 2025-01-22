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
            var locationData = RepositoryStorage.Get<LocationsManager.Data>(KeyConstants.Location);

            if (locationData.LocationName == string.Empty) 
                locationData.LocationName = "World";

            _screenManager.Show(ScreenType.Main);
            _player.gameObject.SetActive(true);
            _gameStateController.StartGame();
            _locationsManager.SwitchLocation(locationData.LocationName, locationData.PointIndex);
        }
    }
}