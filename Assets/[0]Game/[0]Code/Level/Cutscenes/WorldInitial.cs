using System;
using UnityEngine;

namespace Game
{
    public class WorldInitial : MonoBehaviour
    {
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
        }
    }
}