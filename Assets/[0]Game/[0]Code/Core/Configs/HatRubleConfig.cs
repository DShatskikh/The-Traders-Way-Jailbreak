using UnityEngine;
using YG;

namespace Game
{
    [CreateAssetMenu(fileName = "HatRubleConfig", menuName = "Data/Hats/HatRubleConfig", order = 77)]
    public class HatRubleConfig : HatBaseConfig
    {
        private string _price;

        public override string GetPriceText => _price;

        public override void Init()
        {
            foreach (var purchase in YandexGame.purchases)
            {
                if (purchase.id == GetId)
                {
                    _price = purchase.price;
                    return;
                }
            }
        }
    }
}