using UnityEngine;

namespace Game
{
    public class Laptop : MonoBehaviour, IUseObject
    {
        private ScreenManager _screenManager;
        
        private void Awake()
        {
            _screenManager = ServiceLocator.Get<ScreenManager>();
        }

        public void Use()
        {
            _screenManager.Show(ScreenType.Laptop);
        }
    }
}