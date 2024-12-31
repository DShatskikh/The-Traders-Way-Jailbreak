using System;
using System.Collections;
using System.Collections.Generic;
using PixelCrushers.DialogueSystem;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game
{
    public class StockMarketService : MonoBehaviour
    {
        private const float MultiplyStepPercent = 50f;
        private const float MinTarget = 0.3f;
        private const float MaxTarget = 2f;
        
        [SerializeField]
        private DataSlot[] _initSlots;

        private List<Slot> _slots = new();
        public List<Slot> GetSlots => _slots;

        public void Init()
        {
            Lua.RegisterFunction(nameof(AddItem), this,
                SymbolExtensions.GetMethodInfo(() => AddItem(string.Empty, 0d)));
            
            Lua.RegisterFunction(nameof(IsOpenItem), this, 
                SymbolExtensions.GetMethodInfo(() => IsOpenItem(string.Empty)));
            
            foreach (var config in AssetProvider.Instance.StockMarketItems)
            {
                var dataSlot = new DataSlot();

                foreach (var item in _initSlots)
                {
                    if (item.Id == config.Id)
                    {
                        dataSlot = item;
                        break;
                    }
                }

                if (dataSlot.Id == "")
                {
                    dataSlot.Id = config.Id;
                }
                
                var slot = new Slot(dataSlot, config);
                _slots.Add(slot);
            }

            foreach (var slot in _slots) 
                StartCoroutine(AwaitUpdateMultiplySlot(slot));
        }

        public void AddItem(string id, double count = 1)
        {
            foreach (var slot in _slots)
            {
                if (slot.Config.Id != id)
                    continue;

                slot.IsOpen.Value = true;
                slot.Count.Value += (int)count;
                break;
            }
        }

        public bool IsOpenItem(string id)
        {
            foreach (var slot in _slots)
            {
                if (slot.Config.Id == id && slot.IsOpen.Value)
                    return true;
            }
            
            return false;
        }

        private IEnumerator AwaitUpdateMultiplySlot(Slot slot)
        {
            yield return new WaitForSeconds(Random.Range(0f, 0.5f));
            
            while (true)
            {
                slot.PreviousMultiply = slot.Multiply.Value;
                slot.Multiply.Value = Mathf.MoveTowards(slot.Multiply.Value, slot.TargetMultiply, Time.deltaTime * MultiplyStepPercent);

                if (slot.Multiply.Value == slot.TargetMultiply) 
                    slot.TargetMultiply = Random.Range(MinTarget, MaxTarget);

                if (Random.Range(1, 20) == 1)
                {
                    slot.Multiply.Value = Random.Range(MinTarget, MaxTarget);   
                    yield return new WaitForSeconds(0.5f);
                    slot.TargetMultiply = Random.Range(MinTarget, MaxTarget);
                }
                
                //yield return new WaitForSeconds(Random.Range(0.1f, 0.5f));
                yield return new WaitForSeconds(0.5f);
            }
        }

        public class Slot
        {
            private StockMarketItem _config;

            public StockMarketItem Config => _config;
            public ReactiveProperty<float> Multiply = new();
            public ReactiveProperty<int> Count = new();
            public ReactiveProperty<bool> IsOpen = new();
            public float TargetMultiply;
            public float PreviousMultiply;
            
            public float GetPrice => 
                _config.Price + _config.Price * Multiply.Value;
            
            public Slot(DataSlot data, StockMarketItem stockMarketItem)
            {
                _config = stockMarketItem;
                Count.Value = data.Count;
                IsOpen.Value = data.IsOpen;
            }
        }
        
        [Serializable]
        public class DataSlot
        {
            public string Id;
            public int Count;
            public bool IsOpen;
        }
    }
}