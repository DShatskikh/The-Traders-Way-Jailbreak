using PixelCrushers.DialogueSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public sealed class NameSelectScreen : ScreenBase
    {
        private const string DefaultName = "Денис";

        [SerializeField]
        private Button _button, _backButton;

        [SerializeField]
        private TMP_InputField _inputField;

        [SerializeField]
        private Transform _character;

        [SerializeField]
        private GameObject _mainScreen;

        [SerializeField]
        private StartGameScreenBackground _screenBackground;

        [SerializeField]
        private StartGameScreen _startGameScreen;
        
        private LocationLoader _locationLoader;
        private ISaveLoadService _saveLoadService;

        [Inject]
        private void Construct(LocationLoader locationLoader, ISaveLoadService saveLoadService)
        {
            _locationLoader = locationLoader;
            _saveLoadService = saveLoadService;
        }
        
        private void OnEnable()
        {
            _button.onClick.AddListener(OnClick);
            _backButton.onClick.AddListener(OnClickBackButton);
            _character.gameObject.SetActive(true);
            _screenBackground.StartPlayerAnimation();
        }

        private void OnDisable()
        {
            _button.onClick.RemoveAllListeners();
            _backButton.onClick.RemoveAllListeners();
            _character.gameObject.SetActive(false);
            _screenBackground.StartPlayerAnimation();
        }

        private void OnClick()
        {
            _button.onClick.RemoveAllListeners();
            Hide();
            var playerName = _inputField.text;

            if (playerName == string.Empty)
                playerName = DefaultName;
            
            _saveLoadService.Reset();
            
            RepositoryStorage.Set(KeyConstants.Name, new PlayerName(playerName));
            Lua.Run($"Variable[\"PlayerName\"] = \"{playerName}\"");

            Destroy(_mainScreen);
            
            _locationLoader.Load();
        }

        private void OnClickBackButton()
        {
            _startGameScreen.gameObject.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}