using UnityEngine;

namespace Game
{
    public class OpenMainMenuHandler : IGameMainMenuListener
    {
        private ScreenManager _screenManager;
        private Player _player;
        private LocationsManager _levelManager;

        [Inject]
        private void Construct(ScreenManager screenManager, Player player, LocationsManager levelManager)
        {
            _screenManager = screenManager;
            _player = player;
            _levelManager = levelManager;
        }

        public void OnOpenMainMenu()
        {
            _levelManager.DestroyCurrentLocation();
            _screenManager.Hide(ScreenType.Main);
            _screenManager.Hide(ScreenType.Pause);
            _player.gameObject.SetActive(false);
            
            var nameScreen = Object.Instantiate(AssetProvider.Instance.StartGameScreen);

            foreach (var behaviour in nameScreen.GetComponentsInChildren<MonoBehaviour>(true))
                Injector.Inject(behaviour);
        }
    }
}