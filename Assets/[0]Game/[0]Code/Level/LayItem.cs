using PixelCrushers.DialogueSystem;
using UnityEngine;

namespace Game
{
    public class LayItem : MonoBehaviour, IUseObject
    {
        [SerializeField]
        private string _id;
        
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
            _stockMarketService.AddItem(_id);
            _dialogueSystemTrigger.OnUse();
            gameObject.SetActive(false);
        }
    }
}