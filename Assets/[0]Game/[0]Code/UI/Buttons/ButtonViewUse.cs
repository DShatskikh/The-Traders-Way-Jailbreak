using MoreMountains.Feedbacks;
using RimuruDev;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class ButtonViewUse : ButtonViewBase
    {
        [SerializeField]
        private MMF_Player _pressedMmfPlayer;
        
        [SerializeField]
        private MMF_Player _notPressedMmfPlayer;

        [SerializeField]
        private TMP_Text _label;

        [SerializeField]
        private TMP_Text _hint;

        [SerializeField]
        private Transform _view;

        [SerializeField]
        private Image _viewImage;

        private AssetProvider _assetProvider;
        private DeviceTypeDetector _deviceTypeDetector;
        
        private void Awake()
        {
            _assetProvider = AssetProvider.Instance;
            _deviceTypeDetector = ServiceLocator.Get<DeviceTypeDetector>();
        }

        private void Start()
        {
            if (_deviceTypeDetector.CurrentDeviceType == CurrentDeviceType.WebMobile)
                _hint.gameObject.SetActive(false);
        }

        public override void Disable()
        {
            _pressedMmfPlayer.StopFeedbacks();
            _notPressedMmfPlayer.StopFeedbacks();

            _viewImage.color = _assetProvider.DeselectColor;
            _label.color = _assetProvider.DeselectColor;
            _view.transform.localScale = Vector3.one;
        }

        public override void Down()
        {
            _pressedMmfPlayer.PlayFeedbacks();
            _notPressedMmfPlayer.StopFeedbacks();

            _viewImage.color = _assetProvider.SelectColor;
            _label.color = _assetProvider.SelectColor;
        }

        public override void Up()
        {
            _pressedMmfPlayer.StopFeedbacks();
            _notPressedMmfPlayer.PlayFeedbacks();
            
            _viewImage.color = _assetProvider.DeselectColor;
            _label.color = _assetProvider.DeselectColor;
            
            SoundPlayer.Play(_assetProvider.ClickSound);
        }
    }
}