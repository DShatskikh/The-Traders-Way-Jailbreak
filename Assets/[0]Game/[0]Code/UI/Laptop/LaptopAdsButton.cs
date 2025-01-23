using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YG;

namespace Game
{
    public sealed class LaptopAdsButton : Button, IGameLaptopListener
    {
        [SerializeField]
        private TMP_Text _awardLabel;

        [Inject]
        private WalletService _walletService;
        
        public void OnOpenLaptop()
        {
            UpgradeAwardLabel(_walletService.GetMaxMoney);

            _walletService.MaxAwardChanged += UpgradeAwardLabel;
            onClick.AddListener(OnClicked);
        }

        public void OnCloseLaptop()
        {
            _walletService.MaxAwardChanged -= UpgradeAwardLabel;
            onClick.RemoveAllListeners();
        }

        private void UpgradeAwardLabel(double maxMoney)
        {
            _awardLabel.text = $"{_walletService.GetFormatMoney(_walletService.GetMaxAward)}";
        }
        
        private void OnClicked()
        {
            YG2.RewardedAdvShow("Laptop");
        }
    }
}