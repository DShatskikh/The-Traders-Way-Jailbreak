using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;

namespace Game
{
    public sealed class VolumeSlider : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text _label;

        [SerializeField]
        private LocalizedString _localizedString;
        
        private Slider _slider;
        private VolumeService _volumeService;

        private void Awake()
        {
            _slider = GetComponent<Slider>();
            _volumeService = ServiceLocator.Get<VolumeService>();
        }

        private void OnEnable()
        {
            _slider.onValueChanged.AddListener(OnChanged);
            _slider.value = _volumeService.Volume.Value;
        }

        private void OnDisable()
        {
            _slider.onValueChanged.RemoveListener(OnChanged);
        }

        private void Start()
        {
            _slider.value = _volumeService.Volume.Value;
        }

        private void OnChanged(float value)
        {
            _volumeService.Volume.Value = value;
            LocalizedTextUtility.Load(_localizedString, loadText =>
            {
                _label.text = $"{loadText} {(int)(value * 100)}%";
            });
            
            RepositoryStorage.Set(KeyConstants.Volume, new VolumeData() { Volume = value });
        }
    }
}