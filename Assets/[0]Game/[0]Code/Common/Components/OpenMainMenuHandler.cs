using System.Collections;
using UnityEngine;

namespace Game
{
    public class OpenMainMenuHandler : IGameMainMenuListener
    {
        private ScreenManager _screenManager;
        private Player _player;
        private LocationsManager _levelManager;
        private CoroutineRunner _coroutineRunner;

        [Inject]
        private void Construct(ScreenManager screenManager, Player player, LocationsManager levelManager,
            CoroutineRunner coroutineRunner)
        {
            _screenManager = screenManager;
            _player = player;
            _levelManager = levelManager;
            _coroutineRunner = coroutineRunner;
        }

        public void OnOpenMainMenu()
        {
            _coroutineRunner.StartCoroutine(AwaitOpenMenu());
        }

        private IEnumerator AwaitOpenMenu()
        {
            yield return null;
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