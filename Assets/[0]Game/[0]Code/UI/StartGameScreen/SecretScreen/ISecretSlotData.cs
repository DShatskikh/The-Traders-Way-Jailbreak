using UnityEngine;
using UnityEngine.Localization;

namespace Game
{
    public interface ISecretSlotData
    {
        LocalizedString GetName { get; }
        LocalizedString GetDescription { get; }
        Sprite GetIcon { get; }
        Sprite GetPicture { get; }
    }
}