using UnityEngine.UI;

namespace Game
{
    public class PayTaxButton : Button
    {
        private WalletService _walletService;
        
        protected override void Start()
        {
            base.Start();
            _walletService = ServiceLocator.Get<WalletService>();
            _walletService.TaxChanged += WalletServiceOnTaxChanged;
            WalletServiceOnTaxChanged(_walletService.GetTax);
            onClick.AddListener(OnPayTax);
            print("Start");
        }

        private void WalletServiceOnTaxChanged(int tax)
        {
            gameObject.SetActive(tax != 0);
        }

        private void OnPayTax()
        {
            _walletService.TryPayTax();
        }
    }
}