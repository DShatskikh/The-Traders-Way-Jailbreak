using System;
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

        [SerializeField]
        private string _saveId = "default";
        
        private StockMarketService _stockMarketService;
        private SaveData _data;

        [Serializable]
        public struct SaveData
        {
            public bool IsUse;
        }
        
        [Inject]
        private void Construct(StockMarketService stockMarketService)
        {
            _stockMarketService = stockMarketService;
        }

        private void Start()
        {
            _data = CutscenesDataStorage.GetData<SaveData>(_id);

            if (_data.IsUse)
            {
                gameObject.SetActive(false);
            }
        }

        public void Use()
        {
            _stockMarketService.AddItem(_id);
            _dialogueSystemTrigger.OnUse();
            gameObject.SetActive(false);
            
            CutscenesDataStorage.SetData(_id, new SaveData() {IsUse = true});
        }
    }
}