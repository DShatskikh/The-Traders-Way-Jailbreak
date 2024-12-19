using System.Collections;
using UnityEngine;

namespace Game
{
    public class LaptopScreen : BaseScreen
    {
        [SerializeField]
        private StockMarketCell _cellPrefab;

        [SerializeField]
        private Transform _container;
        
        private AssetProvider _assetProvider;

        private void Awake()
        {
            _assetProvider = ServiceLocator.Get<AssetProvider>();
        }

        private IEnumerator Start()
        {
            foreach (var item in _assetProvider.StockMarketItems)
            {
                var cell = Instantiate(_cellPrefab, _container);
                yield return cell.Init(item);
            }
        }
    }
}