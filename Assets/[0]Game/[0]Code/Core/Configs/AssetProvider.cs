using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "AssetProvider", menuName = "Data/AssetProvider", order = 70)]
    public sealed class AssetProvider : ScriptableObject
    {
        public StepSoundPairsConfig StepSoundPairsConfig;
        public TileTagConfig TileTagConfig;
        public AudioClip ClickSound;
        public Color SelectColor;
        public Color DeselectColor;
        public StockMarketItem[] StockMarketItems;
    }
}
