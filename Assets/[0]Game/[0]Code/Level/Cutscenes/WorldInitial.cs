using PixelCrushers.DialogueSystem;
using UnityEngine;

namespace Game
{
    public class WorldInitial : MonoBehaviour
    {
        [SerializeField]
        private DialogueSystemTrigger _startMonolog;
        
        private StockMarketService _stockMarketService;
        private WalletService _walletService;

        [Inject]
        private void Construct(StockMarketService stockMarketService, WalletService walletService)
        {
            _stockMarketService = stockMarketService;
            _walletService = walletService;
        }
        
        private void Start()
        {
            _stockMarketService.OpenAllItems();
            _walletService.Add(13741646);
            
            //_startMonolog.OnUse();
        }
    }
}