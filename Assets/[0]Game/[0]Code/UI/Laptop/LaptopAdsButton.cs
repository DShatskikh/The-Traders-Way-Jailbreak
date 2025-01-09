using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class LaptopAdsButton : Button, IGameLaptopListener
    {
        [SerializeField]
        private TMP_Text _awardLabel;

        [Inject]
        private WalletService _walletService;
        
        public void OnOpenLaptop()
        {
            UpgradeAwardLabel(_walletService.GetMaxMoney);

            _walletService.MaxMoneyChanged += UpgradeAwardLabel;
            onClick.AddListener(OnClicked);
        }

        public void OnCloseLaptop()
        {
            _walletService.MaxMoneyChanged -= UpgradeAwardLabel;
            onClick.RemoveAllListeners();
        }

        private void UpgradeAwardLabel(double maxMoney)
        {
            var award = (int)(maxMoney * 0.5f);

            if (award > 1000000)
                award = 1000000;
            
            _awardLabel.text = $"{_walletService.GetFormatMoney(award)}";
        }
        
        private void OnClicked()
        {
            var maxMoney = _walletService.GetMaxMoney;
            var award = (int)(maxMoney * 0.5f);
            
            if (award > 1000000)
                award = 1000000;
            
            _walletService.Add(award);
        }
    }
}