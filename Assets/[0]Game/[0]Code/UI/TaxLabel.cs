using TMPro;
using UnityEngine;
using UnityEngine.Localization;

namespace Game
{
    public class TaxLabel : MonoBehaviour
    {
        [SerializeField]
        private LocalizedString _localizedString;
        
        private TMP_Text _label;
        private WalletService _walletService;

        private void Awake()
        {
            _label = GetComponent<TMP_Text>();
            _walletService = ServiceLocator.Get<WalletService>();
        }

        private void OnTaxChanged(double tax)
        {
            LocalizedTextUtility.Load(_localizedString, loadText =>
            {
                _label.text = $"{loadText} {_walletService.GetFormatMoney(tax)}";
                Canvas.ForceUpdateCanvases();
            });
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