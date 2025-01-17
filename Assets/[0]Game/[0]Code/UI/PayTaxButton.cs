using UnityEngine.UI;

namespace Game
{
    public sealed class PayTaxButton : Button
    {
        private WalletService _walletService;

        [Inject]
        private void Construct(WalletService walletService)
        {
            _walletService = walletService;
            _walletService.TaxChanged += WalletServiceOnTaxChanged;
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            WalletServiceOnTaxChanged(_walletService.GetTax);
            onClick.AddListener(OnPayTax);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            onClick.RemoveListener(OnPayTax);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _walletService.TaxChanged -= WalletServiceOnTaxChanged;
        }

        private void WalletServiceOnTaxChanged(double tax)
        {
            gameObject.SetActive(tax != 0);
        }

        private void OnPayTax()
        {
            _walletService.TryPayTax();
        }
    }
}