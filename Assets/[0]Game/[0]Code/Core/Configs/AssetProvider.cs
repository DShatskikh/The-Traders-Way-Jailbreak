using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "AssetProvider", menuName = "Data/AssetProvider", order = 70)]
    public sealed class AssetProvider : ScriptableObject
    {
        public StepSoundPairsConfig StepSoundPairsConfig { get; set; }
        public TileTagConfig TileTagConfig { get; set; }
    }
}
