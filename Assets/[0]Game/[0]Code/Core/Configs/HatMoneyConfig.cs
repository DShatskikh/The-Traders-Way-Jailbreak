using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "HatConfig", menuName = "Data/Hats/HatConfig", order = 76)]
    public class HatMoneyConfig : HatBaseConfig
    {
        [SerializeField]
        private double _price;

        public override string GetPriceText => $"${_price}";
        public double GetPrice => _price;
    }
}