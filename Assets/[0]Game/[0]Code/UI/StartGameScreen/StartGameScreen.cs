using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public sealed class StartGameScreen : ScreenBase
    {
        [SerializeField]
        private GameObject _mainScreen;
        
        [SerializeField]
        private NameSelectScreen _nameSelectScreen;
        
        [SerializeField]
        private Button _startGameButton, _newGameButton;

        private LocationLoader _locationLoader;

        [Inject]
        private void Construct(LocationLoader locationLoader)
        {
            _locationLoader = locationLoader;
        }
        
        private void OnEnable()
        {
            _newGameButton.onClick.AddListener(OpenNameSelectScreen);
            _startGameButton.onClick.AddListener(OnContinueButtonClicked);

            _startGameButton.gameObject.SetActive(RepositoryStorage.Get<PlayerName>(KeyConstants.Name).Name != null);
        }

        private void OnDisable()
        {
            _newGameButton.onClick.RemoveListener(OpenNameSelectScreen);
            _startGameButton.onClick.RemoveListener(OnContinueButtonClicked);
        }

        private void OpenNameSelectScreen()
        {
            Hide();
            _nameSelectScreen.Show();
        }

        private void OnContinueButtonClicked()
        {
            _locationLoader.Load();
            Destroy(_mainScreen);
        }
    }
}