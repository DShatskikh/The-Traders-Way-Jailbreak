using System.Collections;
using PixelCrushers.DialogueSystem;
using RimuruDev;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DeviceType = RimuruDev.DeviceType;

namespace Game
{
    public sealed class NameSelectScreen : ScreenBase
    {
        [SerializeField]
        private Button _button, _backButton;

        [SerializeField]
        private TMP_InputField _inputField, _mobileInputField;

        [SerializeField]
        private Transform _character;

        [SerializeField]
        private GameObject _mainScreen, _normal;

        [SerializeField]
        private StartGameScreenBackground _screenBackground;

        [SerializeField]
        private StartGameScreen _startGameScreen;
        
        private LocationLoader _locationLoader;
        private ISaveLoadService _saveLoadService;
        private DeviceTypeDetector _deviceTypeDetector;

        [Inject]
        private void Construct(LocationLoader locationLoader, ISaveLoadService saveLoadService, DeviceTypeDetector deviceTypeDetector)
        {
            _locationLoader = locationLoader;
            _saveLoadService = saveLoadService;
            _deviceTypeDetector = deviceTypeDetector;
        }
        
        private void OnEnable()
        {
            _button.onClick.AddListener(OnClick);
            _backButton.onClick.AddListener(OnClickBackButton);
            _character.gameObject.SetActive(true);
            _screenBackground.StartPlayerAnimation();

            if (_deviceTypeDetector.DeviceType == DeviceType.WebMobile)
            {
                _inputField.onSelect.AddListener(OnSelectMobileInputField);
            }
        }

        private void OnDisable()
        {
            _button.onClick.RemoveAllListeners();
            _backButton.onClick.RemoveAllListeners();
            _character.gameObject.SetActive(false);
            
            _inputField.onSelect.RemoveAllListeners();
        }

        private void OnSelectMobileInputField(string arg0)
        {
            _normal.SetActive(false);
            _mobileInputField.gameObject.SetActive(true);
            StartCoroutine(AwaitSelectMobileInputField());
        }

        private IEnumerator AwaitSelectMobileInputField()
        {
            yield return null;
            _mobileInputField.OnSelect(null);
            yield return new WaitForSeconds(0.5f);
            _mobileInputField.onEndEdit.AddListener(OnDeSelectMobileInputField);
        }

        private void OnDeSelectMobileInputField(string arg0)
        {
            _mobileInputField.onEndEdit.RemoveAllListeners();
            _normal.SetActive(true);
            _mobileInputField.gameObject.SetActive(false);

            _inputField.text = _mobileInputField.text;
        }

        private void OnClick()
        {
            _button.onClick.RemoveAllListeners();
            Hide();
            var playerName = _inputField.text;

            if (playerName == string.Empty)
                playerName = _inputField.placeholder.GetComponent<TMP_Text>().text;
            
            _saveLoadService.Reset();
            
            RepositoryStorage.Set(KeyConstants.Name, new PlayerName(playerName));
            RepositoryStorage.Set(KeyConstants.IsNotFirstOpen, new FirstOpen { IsNotFirstOpen = true });
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