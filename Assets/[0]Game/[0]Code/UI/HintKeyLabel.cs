using System;
using System.Collections;
using RimuruDev;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

namespace Game
{
    public class HintKeyLabel : MonoBehaviour
    {
        [SerializeField]
        private InputActionReference m_Action;

        [SerializeField]
        private string m_BindingId;

        [SerializeField]
        private bool m_isEmptyString;
        
        [SerializeField]
        private LocalizedString m_localizedString;
        
        private TMP_Text _label;
        private string _text;
        private CoroutineRunner _coroutineRunner;
        private DeviceTypeDetector _deviceTypeDetector;

        private void Awake()
        {
            _coroutineRunner = ServiceLocator.Get<CoroutineRunner>();
            _deviceTypeDetector = ServiceLocator.Get<DeviceTypeDetector>();
            _label = GetComponent<TMP_Text>();
        }

        private void OnEnable()
        {
            _coroutineRunner.StartCoroutine(UpdateBindingDisplay());
            LocalizationSettings.SelectedLocaleChanged += LocalizationSettingsOnSelectedLocaleChanged;
        }

        private void OnDisable()
        {
            LocalizationSettings.SelectedLocaleChanged -= LocalizationSettingsOnSelectedLocaleChanged;
        }

        private void Start()
        {
            if (_deviceTypeDetector.CurrentDeviceType == CurrentDeviceType.WebMobile)
                return;
        }

        private void LocalizationSettingsOnSelectedLocaleChanged(Locale obj)
        {
            _coroutineRunner.StartCoroutine(UpdateBindingDisplay());
        }

        private IEnumerator UpdateBindingDisplay()
        {
            var displayString = string.Empty;
            
            var action = m_Action?.action;
            if (action != null)
            {
                var bindingIndex = action.bindings.IndexOf(x => x.id.ToString() == m_BindingId);
                if (bindingIndex != -1)
                    displayString = action.GetBindingDisplayString(bindingIndex, out _, out _);
            }
            
            if (!m_isEmptyString)
            {
                yield return LocalizedTextUtility.AwaitLoad(m_localizedString, (result) =>
                    _label.text = $"{result} [{displayString}]");
            }
            else
            {
                _label.text = $"[{displayString}]";
            }
        }
    }
}