using UnityEngine;
using UnityEngine.Localization;

namespace Game
{
    [CreateAssetMenu(fileName = "HatConstitutumConfig", menuName = "Data/Hats/HatConstitutumConfig", order = 78)]
    public class HatConstitutumConfig : HatBaseConfig
    {
        [SerializeField]
        private LocalizedString _constitutum;

        private string _constitutumString;
        public override string GetPriceText => _constitutumString;

        public override void Init()
        {
            LocalizedTextUtility.Load(_constitutum, (text) => _constitutumString = text);
        }
    }
}