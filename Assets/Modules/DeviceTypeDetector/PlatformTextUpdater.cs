using System;
using Game;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;

namespace RimuruDev
{
    public class PlatformTextUpdater : MonoBehaviour
    {
        [SerializeField]
        private LocalizedString _pcText;
        
        [SerializeField]
        private LocalizedString _androidText;

        private TMP_Text _label;
        private DeviceTypeDetector _deviceTypeDetector;

        [Inject]
        private void Construct(DeviceTypeDetector deviceTypeDetector)
        {
            _deviceTypeDetector = deviceTypeDetector;
            _label = GetComponent<TMP_Text>();
        }
        
        private void Start()
        {
            var localizedString = _deviceTypeDetector.DeviceType switch
            {
                DeviceType.WebPC => _pcText,
                DeviceType.WebMobile => _androidText,
                _ => throw new ArgumentOutOfRangeException()
            };
            
            LocalizedTextUtility.Load(localizedString, text => _label.text = text);
        }
    }
}