using System;
using PixelCrushers.DialogueSystem;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class WalletService
    {
        private const double MaxAward = 100000000000; //100 миллиардов
        
        [SerializeField]
        private double _money;

        private double _tax;
        private double _maxMoney = 5;
        
        public double GetMoney => _money;
        public double GetMaxMoney => _maxMoney;
        public double GetMaxAward => _maxMoney > MaxAward ? MaxAward : _maxMoney * 0.5f;
        public double GetTax => _tax;

        public event Action<double> Changed;
        public event Action<double> TaxChanged;
        public event Action<double> MaxAwardChanged;

        public void Init()
        {
            Lua.RegisterFunction(nameof(IsHaveMoney), this,
                SymbolExtensions.GetMethodInfo(() => IsHaveMoney(0d)));
            
            Lua.RegisterFunction(nameof(MinusMoney), this,
                SymbolExtensions.GetMethodInfo(() => MinusMoney(0d)));
            
            Lua.RegisterFunction(nameof(GetTaxString), this,
                SymbolExtensions.GetMethodInfo(() => GetTaxString()));
        }

        public bool TryBuy(double price)
        {
            if (_money < price)
            {
                SoundPlayer.Play(AssetProvider.Instance.BruhSound);
                return false;
            }
            
            _money -= price;
            Changed?.Invoke(_money);
            SoundPlayer.Play(AssetProvider.Instance.BuySound);
            return true;
        }

        public void Add(double price)
        {
            if (_money + price > double.MaxValue)
            {
                _money = double.MaxValue;
                return;
            }

            if (_money + price > double.MaxValue)
            {
                _money = double.MaxValue;
            }
            else
            {
                _money += price;
            }
            
            Changed?.Invoke(_money);
            _tax += price / 100f;
            TaxChanged?.Invoke(_tax);
            Lua.Run($"Variable[\"Tax\"] = \"{GetTaxString()}\"");

            if (_money > _maxMoney)
            {
                _maxMoney = _money;
                MaxAwardChanged?.Invoke(GetMaxAward);
            }
        }

        public bool TryPayTax()
        {
            if (TryBuy(_tax))
            {
                _tax = 0;
                TaxChanged?.Invoke(_tax);
                Lua.Run($"Variable[\"Tax\"] = \"{GetTaxString()}\"");
                return true;
            }
            
            return false;
        }
        
        public string GetFormatMoney(double money)
        {
            if (money > 999999999999999) //999 триллионов
                return $"${999},{99}aa";
                         
            if (money >= 1000000000000)
                return $"${Math.Round(money / 1000000000000, 2)}T";
            
            if (money >= 1000000000)
                return $"${Math.Round(money / 1000000000, 2)}B";
            
            if (money >= 1000000)
                    return $"${Math.Round(money / 1000000, 2)}M";
            
            if (money >= 1000)
                return $"${Math.Round(money / 1000, 2)}K";
            
            return $"${Math.Round(money, 2)}";
        }

        public void Reset()
        {
            _money = 0;
            _tax = 0;
            
            Changed?.Invoke(_money);
            TaxChanged?.Invoke(_tax);
            
            Lua.Run($"Variable[\"Tax\"] = \"{GetTaxString()}\"");
        }

        public void SetMoneyAndTax(double money, double tax)
        {
            _money = money;
            _tax = tax;
            
            Changed?.Invoke(_money);
            TaxChanged?.Invoke(_tax);
            
            Lua.Run($"Variable[\"Tax\"] = \"{GetTaxString()}\"");
        }
        
        private bool IsHaveMoney(double money) => 
            _money >= money;
        
        private void MinusMoney(double money) => 
            _money -= money;

        private string GetTaxString() => 
            GetFormatMoney(_tax);
    }
}