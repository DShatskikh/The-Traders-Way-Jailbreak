using UnityEngine;
using UnityEngine.Localization;

namespace Game
{
    public abstract class HatBaseConfig : ScriptableObject
    {
        [SerializeField]
        private string _id;

        [SerializeField]
        private LocalizedString _name;

        public string GetId => _id;
        public virtual LocalizedString GetName => _name;
        public abstract string GetPriceText { get; }

        public virtual void Init() { }
    }
}