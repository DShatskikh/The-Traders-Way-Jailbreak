using UnityEngine;
using UnityEngine.Localization;

namespace Game
{
    [CreateAssetMenu(fileName = "SecretSlot", menuName = "Data/SecretSlot", order = 100)]
    public sealed class SecretSlotData : ScriptableObject, ISecretSlotData
    {
        [SerializeField]
        private LocalizedString _name;

        [SerializeField]
        private LocalizedString _description;
        
        [SerializeField]
        private Sprite _icon;
        
        [SerializeField]
        private Sprite _picture;
        
        public LocalizedString GetName => _name;
        public LocalizedString GetDescription => _description;
        public Sprite GetIcon => _icon;
        public Sprite GetPicture => _picture;
    }
}