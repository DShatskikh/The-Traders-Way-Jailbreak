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
        private Button _button;

        [SerializeField]
        private TMP_InputField _inputField;

        [SerializeField]
        private Transform _character;

        [SerializeField]
        private GameObject _mainScreen;
        
        private ScreenManager _screenManager;
        private LocationsManager _locationsManager;
        private GameStateController _gameStateController;
        private Player _player;
        private LocationLoader _locationLoader;

        [Inject]
        private void Construct(ScreenManager screenManager, LocationsManager locationsManager,
            GameStateController gameStateController, Player player, LocationLoader locationLoader)
        {
            _screenManager = screenManager;
            _locationsManager = locationsManager;
            _gameStateController = gameStateController;
            _player = player;
            _locationLoader = locationLoader;
        }
        
        private void Start()
        {
            _button.onClick.AddListener(OnClick);
            _character.gameObject.SetActive(true);
        }

        private void OnClick()
        {
            _button.onClick.RemoveAllListeners();
            Hide();
            var playerName = _inputField.text;

            if (playerName == string.Empty)
                playerName = DefaultName;
            
            RepositoryStorage.Set(KeyConstants.Name, new PlayerName(playerName));
            Lua.Run($"Variable[\"PlayerName\"] = \"{playerName}\"");

            Destroy(_mainScreen);
            _locationLoader.Load();
        }
    }
}