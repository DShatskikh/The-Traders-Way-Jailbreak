using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YG;

namespace Game
{
    public sealed class SkinShopScreen : ScreenBase, IGameShopListener
    {
        [SerializeField]
        private TMP_Text _nameLabel;
        
        [SerializeField]
        private TMP_Text _priceLabel;

        [SerializeField]
        private TMP_Text _buyLabel;
        
        [SerializeField]
        private Button _rightButton, _leftButton, _buyButton;

        [SerializeField]
        private Button _exitButton;

        private GameStateController _gameStateController;
        private HatManager _hatManager;
        private HatData _reallyCurrentHat;
        private int _indexHat;
        private WalletService _walletService;

        [Inject]
        private void Construct(HatManager hatManager, GameStateController gameStateController, WalletService walletService)
        {
            _hatManager = hatManager;
            _gameStateController = gameStateController;
            _walletService = walletService;
        }

        public override void Show()
        {
            base.Show();
            
            _reallyCurrentHat = _hatManager.GetCurrentHat;

            for (int i = 0; i < _hatManager.GetAllHat.Count(); i++)
            {
                if (_hatManager.GetAllHat.ToArray()[i] == _hatManager.GetCurrentHat)
                {
                    _indexHat = i;
                    break;
                }
            }
            
            OnHatUpgrade(_hatManager.GetCurrentHat);
            _hatManager.OnHatUpgrade += OnHatUpgrade;
            
            _rightButton.onClick.AddListener(OnLeftButtonClicked);
            _leftButton.onClick.AddListener(OnRightButtonClicked);
            _buyButton.onClick.AddListener(OnButButtonClicked);
            _exitButton.onClick.AddListener(_gameStateController.CloseShop);
        }


        public override void Hide()
        {
            base.Hide();
            
            _rightButton.onClick.RemoveAllListeners();
            _leftButton.onClick.RemoveAllListeners();
            _buyButton.onClick.RemoveAllListeners();
            _exitButton.onClick.RemoveAllListeners();
        }

        public void OnOpenShop()
        {
            Show();
        }

        public void OnCloseShop()
        {
            Hide();
            _hatManager.UpgradeHat(_reallyCurrentHat);
        }

        private void OnLeftButtonClicked()
        {
            ++_indexHat;

            if (_indexHat >= _hatManager.GetAllHat.Count())
                _indexHat = _hatManager.GetAllHat.Count() - 1;
            
            _hatManager.UpgradeHat(_hatManager.GetAllHat.ToArray()[_indexHat]);
        }

        private void OnRightButtonClicked()
        {
            --_indexHat;

            if (_indexHat < 0)
                _indexHat = 0;

            _hatManager.UpgradeHat(_hatManager.GetAllHat.ToArray()[_indexHat]);
        }

        private void OnButButtonClicked()
        {
            var hat = _hatManager.GetCurrentHat;
            
            if (hat != null)
            {
                if (hat.IsBuy)
                {
                    _reallyCurrentHat = hat;
                    OnHatUpgrade(hat);
                }
                else
                {
                    switch (hat.Config)
                    {
                        case HatMoneyConfig moneyConfig:
                        {
                            if (_walletService.TryBuy(moneyConfig.GetPrice))
                            {
                                _hatManager.BuyHat(hat.Config.GetId);
                                _reallyCurrentHat = _hatManager.GetCurrentHat;
                                OnHatUpgrade(hat);
                            }

                            break;
                        }
                        case HatRubleConfig rubleConfig:
                            _gameStateController.CloseShop();
                            YG2.BuyPayments(rubleConfig.GetId);
                            break;
                        case HatAdsConfig adsConfig:
                            _gameStateController.CloseShop();
                            YG2.RewardedAdvShow(adsConfig.GetAdsIndex);
                            break;  
                    }
                }
            }
            else
            {
                _reallyCurrentHat = null;
                OnHatUpgrade(null);
            }
        }

        private void OnHatUpgrade(HatData data)
        {
            _leftButton.gameObject.SetActive(_indexHat != 0);
            _rightButton.gameObject.SetActive(_indexHat != _hatManager.GetAllHat.Count() - 1);

            _buyButton.gameObject.SetActive(true);
            
            if (data == null)
            {
                _priceLabel.text = "Выбрано";
                _nameLabel.text = "Стандартный";
            }
            else
            {
                _priceLabel.text = data.Config.GetPriceText;
                LocalizedTextUtility.Load(data.Config.GetName, (text) => _nameLabel.text = text);
            }
            
            if (_reallyCurrentHat == data)
            {
                _priceLabel.text = "Выбрано";
                _buyButton.gameObject.SetActive(false);
            }
            else if (data == null)
            {
                _priceLabel.text = string.Empty;
                _buyLabel.text = "Выбрать";
            }
            else
            {
                if (data.IsBuy)
                {
                    _priceLabel.text = string.Empty;
                    _buyLabel.text = "Выбрать";
                }
                else
                {
                    if (data.Config is HatConstitutumConfig)
                    {
                        _buyButton.gameObject.SetActive(false);
                    }
                    else
                    {
                        _buyLabel.text = "Купить";
                    }
                }
            }
        }
    }
}