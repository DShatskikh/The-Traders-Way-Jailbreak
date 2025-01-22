using UnityEngine;
using UnityEngine.InputSystem;
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
        private Button _startGameButton, _newGameButton, _secretButton;

        [SerializeField]
        private SecretView _secretView;

        [SerializeField]
        private SecretSlotView _secretSlotView;
        
        private LocationLoader _locationLoader;
        private ISecretPresenter _secretPresenter;

        [Inject]
        private void Construct(LocationLoader locationLoader, PlayerInput playerInput)
        {
            _locationLoader = locationLoader;
            var slots = Resources.LoadAll<SecretSlotData>("SecretSlotData");
            _secretPresenter = new SecretPresenter(_secretView, slots, _secretSlotView, this, playerInput);
        }
        
        private void OnEnable()
        {
            _newGameButton.onClick.AddListener(OpenNameSelectScreen);
            _startGameButton.onClick.AddListener(OnContinueButtonClicked);

            var endingsData = RepositoryStorage.Get<EndsData>(KeyConstants.Ending);
            _secretButton.gameObject.SetActive(endingsData.IsDefaultEnding);
            
            _secretButton.onClick.AddListener(OnSecretButtonClicked);

            _startGameButton.gameObject.SetActive(RepositoryStorage.Get<PlayerName>(KeyConstants.Name).Name != null);
        }

        private void OnDisable()
        {
            _newGameButton.onClick.RemoveAllListeners();
            _startGameButton.onClick.RemoveAllListeners();
            _secretButton.onClick.RemoveAllListeners();
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

        private void OnSecretButtonClicked()
        {
            Hide();
            _secretPresenter.Show();
        }
    }
}