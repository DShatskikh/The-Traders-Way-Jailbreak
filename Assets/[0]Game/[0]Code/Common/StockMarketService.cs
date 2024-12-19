using System;
using System.Collections.Generic;

namespace Game
{
    [Serializable]
    public class StockMarketService
    {
        private Dictionary<StockMarketItem, DataSlot> _slotsPair = new();

        public void Init()
        {
            
        }
        
        [Serializable]
        public class Data
        {
            public DataSlot[] Slots;
            public int Tax;
        }
        
        [Serializable]
        public class DataSlot
        {
            public string Id;
            public int Multiply;
            public int Count;
        }
    }
}