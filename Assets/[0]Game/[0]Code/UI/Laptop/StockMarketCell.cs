using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class StockMarketCell : MonoBehaviour
    {
        [SerializeField]
        private Image _frame;

        [SerializeField]
        private Image _icon;

        [SerializeField]
        private TMP_Text _nameLabel;
        
        [SerializeField]
        private TMP_Text _priceLabel;
        
        [SerializeField]
        private TMP_Text _haveLabel;

        [SerializeField]
        private GameObject _close;

        [SerializeField]
        private Button _buyButton;

        [SerializeField]
        private Button _sellButton;

        [SerializeField]
        private TMP_Dropdown _dropdown;
        
        private StockMarketItem _config;
        private WalletService _walletService;
        private StockMarketService.Slot _slot;

        private void Awake()
        {
            _walletService = ServiceLocator.Get<WalletService>();
        }

        public IEnumerator Init(StockMarketService.Slot slot)
        {
            _slot = slot;
            
            slot.Count.Changed += CountOnChanged;
            slot.IsOpen.Changed += IsOpenOnChanged;
            slot.Multiply.Changed += MultiplyOnChanged;
            
            var item = slot.Config;
            _config = item;
            _icon.sprite = item.Icon;

            UpdatePrice();
            CountOnChanged(slot.Count.Value);
            IsOpenOnChanged(slot.IsOpen.Value);

            yield return LocalizedTextUtility.AwaitLoad(item.Name, (text) => _nameLabel.text = text);
            
            _buyButton.onClick.AddListener(OnBuy);
            _sellButton.onClick.AddListener(OnSell);
            
            _walletService.Changed += WalletServiceOnChanged;
            _dropdown.onValueChanged.AddListener(OnDropdownValueChanged);
        }

        private void OnDropdownValueChanged(int count) => 
            SoundPlayer.Play(AssetProvider.Instance.ClickSound);

        private void MultiplyOnChanged(float multiply) => 
            UpdatePrice();

        private void UpdatePrice()
        {
            var text = $"{_walletService.GetFormatMoney(_slot.GetPrice)} ";
            
            var multiply = (int)(_slot.Multiply.Value * 100);
            
            if (multiply > 0)
                text += $"<Color=\"green\">+{multiply}%";
            else
                text += $"<Color=\"red\">{multiply}%";
            
            if (_slot.PreviousMultiply < _slot.Multiply.Value)
                text += $"<sprite name=\"Up\">";
            else
                text += $"<sprite name=\"Down\">";
            
            //print(text);
            _priceLabel.text = text;
        }
        
        private void IsOpenOnChanged(bool isOpen)
        {
            _close.SetActive(!isOpen);
        }

        private void CountOnChanged(int count)
        {
            _haveLabel.text = $"У ВАС: {count} ШТ";
            _sellButton.interactable = count >= GetSellCount();
        }

        private void WalletServiceOnChanged(float money)
        {
            _buyButton.interactable = money >= GetPrice();
        }

        private void OnBuy()
        {
            if (_walletService.TryBuy(GetPrice()))
                _slot.Count.Value += GetSellCount();
        }
        
        private void OnSell()
        {
            if (_slot.Count.Value < GetSellCount())
                return;
            
            _slot.Count.Value -= GetSellCount();
            _walletService.Add(GetPrice());
            SoundPlayer.Play(AssetProvider.Instance.BuySound);
        }

        private int GetSellCount() => 
            int.Parse(_dropdown.options[_dropdown.value].text);
        
        private float GetPrice() => 
            _slot.GetPrice * GetSellCount();
    }
}