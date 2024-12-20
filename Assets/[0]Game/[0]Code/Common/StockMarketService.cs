using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game
{
    public class StockMarketService : MonoBehaviour
    {
        [SerializeField]
        private DataSlot[] _initSlots;

        private List<Slot> _slots = new();
        public List<Slot> GetSlots => _slots;

        private void Start()
        {
            foreach (var slot in _slots)
            {
                StartCoroutine(AwaitUpdateMultiplySlot(slot));
            }
        }

        private IEnumerator AwaitUpdateMultiplySlot(Slot slot)
        {
            while (true)
            {
                slot.PreviousMultiply = slot.Multiply.Value;
                slot.Multiply.Value = Mathf.MoveTowards(slot.Multiply.Value, slot.TargetMultiply, Time.deltaTime);

                if (slot.Multiply.Value == slot.TargetMultiply) 
                    slot.TargetMultiply = Random.Range(0.75f, 1.2f);

                if (Random.Range(1, 20) == 19)
                {
                    slot.Multiply.Value += Random.Range(-0.2f, 0.2f);   
                    yield return new WaitForSeconds(0.5f);
                    slot.TargetMultiply = Random.Range(0.75f, 1.2f);
                }
                
                yield return new WaitForSeconds(Random.Range(0.025f, 0.1f));
            }
        }

        public void Init()
        {
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
        }

        public class Slot
        {
            public Slot(DataSlot data, StockMarketItem stockMarketItem)
            {
                _config = stockMarketItem;
                Count.Value = data.Count;
                IsOpen.Value = data.IsOpen;
            }
            
            private StockMarketItem _config;
            
            public StockMarketItem Config => _config;
            public ReactiveProperty<float> Multiply = new ReactiveProperty<float>();
            public ReactiveProperty<int> Count = new ReactiveProperty<int>();
            public ReactiveProperty<bool> IsOpen = new ReactiveProperty<bool>();
            public float TargetMultiply;
            public float PreviousMultiply;
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