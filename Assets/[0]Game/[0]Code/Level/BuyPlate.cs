using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;

namespace Game
{
    public sealed class BuyPlate : MonoBehaviour, IUseObject
    {
        [SerializeField]
        private UnityEvent _buyEvent;

        [SerializeField]
        private int _price = 100;

        [SerializeField]
        private LocalizedString _name;

        [SerializeField]
        private string _id = "default";
        
        [SerializeField]
        private TMP_Text _label;
        
        private WalletService _walletService;
        private SaveData _data;

        public static Action BuyEvent;

        [Serializable]
        public struct SaveData
        {
            public bool IsBuy;
        }
        
        [Inject]
        private void Construct(WalletService walletService)
        {
            _walletService = walletService;
        }

        private void Start()
        {
            LocalizedTextUtility.Load(_name, (result) => _label.text = $"{result}\n${_price}");
            
            _data = RepositoryStorage.Get<SaveData>(_id);

            if (_data.IsBuy)
            {
                _buyEvent.Invoke();
                gameObject.SetActive(false);
            }
        }

        public void Use()
        {
            if (_walletService.TryBuy(_price))
            {
                SoundPlayer.Play(AssetProvider.Instance.BuySound);
                _buyEvent.Invoke();
                gameObject.SetActive(false);
                
                RepositoryStorage.Set(_id, new SaveData() {IsBuy = true});
                BuyEvent?.Invoke();
            }
            else
            {
                SoundPlayer.Play(AssetProvider.Instance.BruhSound);
            } 
        }
    }
}