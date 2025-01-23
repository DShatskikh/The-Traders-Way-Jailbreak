using System.Linq;
using UnityEngine;
using UnityEngine.Localization.Settings;
using YG;

namespace Game
{
    public static class CorrectLang
    {
        /*[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Init()
        {
            YG2.onCorrectLang += OnСhangeLang;
        }*/

        public static void OnСhangeLang(string lang)
        {
            Debug.Log($"Язык сменен на {lang}");
            
            var localeQuery = (from locale in LocalizationSettings.AvailableLocales.Locales where locale.Identifier.Code == lang select locale).FirstOrDefault();
            if (localeQuery == null)
            {
                Debug.LogError($"No locale for {lang} found");
                return;
            }
            LocalizationSettings.SelectedLocale = localeQuery;
        }
    }
}