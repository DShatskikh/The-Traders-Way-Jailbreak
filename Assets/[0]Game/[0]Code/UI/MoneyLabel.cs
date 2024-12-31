using System;
using TMPro;
using UnityEngine;

namespace Game
{
    public class MoneyLabel : MonoBehaviour
    {
        private TMP_Text _label;
        private WalletService _walletService;

        private void Awake()
        {
            _label = GetComponent<TMP_Text>();
            _walletService = ServiceLocator.Get<WalletService>();
        }

        private void WalletServiceOnChanged(float money)
        {
            _label.text = _walletService.GetFormatMoney(money);
        }

        private void OnEnable()
        {
            _walletService.Changed += WalletServiceOnChanged;
            WalletServiceOnChanged(_walletService.GetMoney);
        }

        private void OnDisable()
        {
            _walletService.Changed -= WalletServiceOnChanged;
        }
    }
}