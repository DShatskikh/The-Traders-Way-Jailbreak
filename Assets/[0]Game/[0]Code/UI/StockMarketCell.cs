using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class StockMarketCell : MonoBehaviour
    {
        [SerializeField]
        private Image _frame;

        [SerializeField]
        private Image _icon;

        [SerializeField]
        private TMP_Text _nameLabel;
        
        [SerializeField]
        private TMP_Text _priceLabel;
        
        [SerializeField]
        private TMP_Text _haveLabel;

        [SerializeField]
        private GameObject _close;
        
        private AssetProvider _assetProvider;
        private StockMarketItem _config;

        private void Awake()
        {
            _assetProvider = ServiceLocator.Get<AssetProvider>();
        }

        private void OnEnable()
        {
            
        }

        private void OnDisable()
        {
            
        }

        public IEnumerator Init(StockMarketItem item)
        {
            _config = item;
            _icon.sprite = item.Icon;
            _priceLabel.text = $"{item.Price} <Color=\"green\">+100";
            _haveLabel.text = "У ВАС: 0";
            _close.SetActive(false);

            yield return LocalizedTextUtility.AwaitLoad(item.Name, (text) =>
            {
                print(item.Name);
                print(text);
                _nameLabel.text = text;
            });
        }
    }
}