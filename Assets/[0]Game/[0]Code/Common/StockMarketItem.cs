using System;
using UnityEngine;
using UnityEngine.Localization;

namespace Game
{
    [Serializable]
    public struct StockMarketItem
    {
        public string Id;
        public Sprite Icon;
        public int Price;
        public LocalizedString Name;
    }
}