using System;
using System.Collections;
using UnityEngine.Localization;

namespace Game
{
    public static class LocalizedTextUtility
    {
        public static IEnumerator AwaitLoad(LocalizedString localizedString, Action<string> onComplete)
        {
            var textOperation = localizedString.GetLocalizedStringAsync();
                
            while (!textOperation.IsDone)
                yield return null;

            string t = textOperation.Result;
            onComplete.Invoke(t);
        }

        public static void Load(LocalizedString localizedString, Action<string> onComplete)
        {
            ServiceLocator.Get<CoroutineRunner>().StartCoroutine(AwaitLoad(localizedString, onComplete));
        }
    }
}