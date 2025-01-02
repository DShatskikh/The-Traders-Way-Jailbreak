using UnityEngine;

namespace Game
{
    public class PrehistoricVase : MonoBehaviour, IUseObject
    {
        [SerializeField]
        private GameObject _vase;

        [SerializeField]
        private GameObject _replace;

        private bool _isReplace;
        
        public void Use()
        {
            
        }

        public void Replace()
        {
            
        }
    }
}