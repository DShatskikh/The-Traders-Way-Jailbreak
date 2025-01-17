using UnityEngine;
using UnityEngine.Localization;

namespace Game
{
    [CreateAssetMenu(fileName = "HatAdsConfig", menuName = "Data/Hats/HatAdsConfig", order = 79)]
    public class HatAdsConfig : HatBaseConfig
    {
        [SerializeField]
        private LocalizedString _ads;

        [SerializeField]
        private int _adsIndex;
        
        private string _adsString;
        public override string GetPriceText => _adsString;
        public int GetAdsIndex => _adsIndex;

        public override void Init()
        {
            LocalizedTextUtility.Load(_ads, (text) => _adsString = text);
        }
    }
}