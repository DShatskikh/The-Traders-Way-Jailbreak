using PixelCrushers.DialogueSystem;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

namespace Game
{
    public class DialogueLanguageSwitcher : MonoBehaviour
    {
        private void Awake()
        {
            print(LocalizationSettings.SelectedLocale.Identifier.Code);
            DialogueManager.SetLanguage(LocalizationSettings.SelectedLocale.Identifier.Code);
            LocalizationSettings.SelectedLocaleChanged += LocalizationSettingsOnSelectedLocaleChanged;
        }

        private void OnDestroy()
        {
            LocalizationSettings.SelectedLocaleChanged -= LocalizationSettingsOnSelectedLocaleChanged;
        }

        private void LocalizationSettingsOnSelectedLocaleChanged(Locale language)
        {
            print(language.Identifier.Code);
            DialogueManager.SetLanguage(language.Identifier.Code);
        }
    }
}