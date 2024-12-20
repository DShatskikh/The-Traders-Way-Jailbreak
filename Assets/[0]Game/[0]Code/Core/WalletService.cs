using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class WalletService
    {
        [SerializeField]
        private int _money;

        private float _tax;

        public int GetMoney => _money;
        public int GetTax => (int)_tax;

        public event Action<int> Changed;
        public event Action<int> TaxChanged;

        public bool TryBuy(int price)
        {
            if (_money < price)
                return false;
            
            _money -= price;
            Changed?.Invoke(_money);
            return true;
        }

        public void Add(int price)
        {
            _money += price;
            Changed?.Invoke(_money);
            _tax += price / 100f;
            TaxChanged?.Invoke((int)_tax);
        }

        public bool TryPayTax()
        {
            if (TryBuy((int)_tax))
            {
                _tax = 0;
                TaxChanged?.Invoke((int)_tax);
                return true;
            }
            
            return false;
        }
        
        public string GetFormatMoney(int money)
        {
            if (money > 1000000)
                    return $"${money / 1000000}M";
            
            if (money > 1000)
                return $"${money / 1000}K";
            
            return $"${money}";
        }
    }
}