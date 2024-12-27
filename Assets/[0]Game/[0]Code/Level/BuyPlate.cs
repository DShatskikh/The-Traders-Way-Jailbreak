using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;

namespace Game
{
    public class BuyPlate : MonoBehaviour, IUseObject
    {
        [SerializeField]
        private UnityEvent _buyEvent;

        [SerializeField]
        private int _price = 100;

        [SerializeField]
        private LocalizedString _name;

        private WalletService _walletService;
        
        private void Awake()
        {
            _walletService = ServiceLocator.Get<WalletService>();
        }

        public void Use()
        {
            if (_walletService.TryBuy(_price))
            {
                SoundPlayer.Play(AssetProvider.Instance.BuySound);
                _buyEvent.Invoke();
                gameObject.SetActive(false);
            }
            else
            {
                SoundPlayer.Play(AssetProvider.Instance.BruhSound);
            } 
        }
    }
}