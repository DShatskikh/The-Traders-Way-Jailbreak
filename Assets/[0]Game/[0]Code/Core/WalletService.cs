using System;
using PixelCrushers.DialogueSystem;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class WalletService
    {
        [SerializeField]
        private double _money;

        private double _tax;
        private double _maxMoney = 5;
        
        public double GetMoney => _money;
        public double GetMaxMoney => _maxMoney;
        public double GetTax => _tax;

        public event Action<double> Changed;
        public event Action<double> TaxChanged;
        public event Action<double> MaxMoneyChanged;

        public void Init()
        {
            Lua.RegisterFunction(nameof(IsHaveMoney), this,
                SymbolExtensions.GetMethodInfo(() => IsHaveMoney(0d)));
            
            Lua.RegisterFunction(nameof(MinusMoney), this,
                SymbolExtensions.GetMethodInfo(() => MinusMoney(0d)));
        }

        public bool TryBuy(float price)
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

        public void Add(float price)
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

            if (_money > _maxMoney)
            {
                _maxMoney = _money;
                MaxMoneyChanged?.Invoke(_maxMoney);
            }
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
        
        public string GetFormatMoney(double money)
        {
            if (money > 999999999999999) //999 триллионов
                return $"${999},{99}Т";
                         
            if (money >= 1000000000000)
                return $"${Math.Round(money / 1000000000000, 2)}Т";
            
            if (money >= 1000000000)
                return $"${Math.Round(money / 1000000000, 2)}ММ";
            
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
        }

        public void SetMoneyAndTax(int money, int tax)
        {
            _money = money;
            _tax = tax;
            
            Changed?.Invoke(_money);
            TaxChanged?.Invoke(_tax);
        }
        
        private bool IsHaveMoney(double money) => 
            _money >= money;
        
        private void MinusMoney(double money) => 
            _money -= (float)money;
    }
}