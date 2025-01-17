using UnityEngine;

namespace Game
{
    public sealed class Watermelon : MonoBehaviour
    {
        [SerializeField]
        private GameObject _watermelon;
        
        private StockMarketService _stockMarketService;

        [Inject]
        private void Construct(StockMarketService stockMarketService)
        {
            _stockMarketService = stockMarketService;
        }
        
        private void Start()
        {
            if (_stockMarketService.IsOpenItem("Watermelon"))
                Hide();
        }

        private void Hide()
        {
            _watermelon.SetActive(false);
        }
    }
}