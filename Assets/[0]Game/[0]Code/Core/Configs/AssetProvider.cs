using UnityEngine;
using UnityEngine.Serialization;

namespace Game
{
    [CreateAssetMenu(fileName = "AssetProvider", menuName = "Data/AssetProvider", order = 70)]
    public sealed class AssetProvider : ScriptableObject
    {
        public void Init()
        {
            Instance = this;
        }

        public static AssetProvider Instance { get; private set; }

        [Header("Configs")]
        public StepSoundPairsConfig StepSoundPairsConfig;
        public TileTagConfig TileTagConfig;
        public Color SelectColor;
        public Color DeselectColor;
        public StockMarketItem[] StockMarketItems;
        public HatBaseConfig[] HatConfigs;
        public string[] PlatesId;
        
        [Header("AudioClips")]
        public AudioClip BuySound;
        public AudioClip ClickSound;
        public AudioClip BruhSound;
        public AudioClip BreakSound;
        public AudioClip SpaceDoorOpenSound;
        public AudioClip SpaceDoorCloseSound;

        [Header("GameObjects")]
        public GameObject StartGameScreen;
    }
}
