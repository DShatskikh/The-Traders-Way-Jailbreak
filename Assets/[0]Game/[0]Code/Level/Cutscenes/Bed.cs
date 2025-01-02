using UnityEngine;

namespace Game
{
    public class Bed : MonoBehaviour, IUseObject
    {
        [SerializeField]
        private HomeCutscene _homeCutscene;
        
        public void Use()
        {
            _homeCutscene.Sleep();
        }
    }
}