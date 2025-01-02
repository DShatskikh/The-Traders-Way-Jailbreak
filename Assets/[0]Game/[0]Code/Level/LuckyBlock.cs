using PixelCrushers.DialogueSystem;
using UnityEngine;

namespace Game
{
    public class LuckyBlock : MonoBehaviour, IUseObject
    {
        [SerializeField]
        private DialogueSystemTrigger _dialogueSystemTrigger;

        private StockMarketService _stockMarketService;

        [Inject]
        private void Construct(StockMarketService stockMarketService)
        {
            _stockMarketService = stockMarketService;
        }
        
        public void Use()
        {
            _dialogueSystemTrigger.OnUse();
            _stockMarketService.AddItem("LuckyBlock");
            gameObject.SetActive(false);
        }
    }
}