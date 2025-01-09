using TMPro;
using UnityEngine;

namespace Game
{
    public class TaxLabel : MonoBehaviour
    {
        private TMP_Text _label;
        private WalletService _walletService;

        private void Awake()
        {
            _label = GetComponent<TMP_Text>();
            _walletService = ServiceLocator.Get<WalletService>();
        }

        private void OnTaxChanged(double tax)
        {
            _label.text = $"Налог: {_walletService.GetFormatMoney(tax)}";
            Canvas.ForceUpdateCanvases();
        }

        private void OnEnable()
        {
            _walletService.TaxChanged += OnTaxChanged;
            OnTaxChanged(_walletService.GetTax);
        }

        private void OnDisable()
        {
            _walletService.TaxChanged -= OnTaxChanged;
        }
    }
}