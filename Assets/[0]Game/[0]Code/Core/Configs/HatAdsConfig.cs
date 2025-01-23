using UnityEngine;
using UnityEngine.Localization;

namespace Game
{
    [CreateAssetMenu(fileName = "HatAdsConfig", menuName = "Data/Hats/HatAdsConfig", order = 79)]
    public class HatAdsConfig : HatBaseConfig
    {
        [SerializeField]
        private LocalizedString _ads;

        private string _adsString;
        public override string GetPriceText => _adsString;
        public string GetAdsIndex => GetId;

        public override void Init()
        {
            LocalizedTextUtility.Load(_ads, (text) => _adsString = text);
        }
    }
}