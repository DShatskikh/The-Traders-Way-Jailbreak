using PixelCrushers.DialogueSystem;
using UnityEngine;

namespace Game
{
    public class Rocket : MonoBehaviour, IUseObject
    {
        [SerializeField]
        private DialogueSystemTrigger _dialogueSystemTrigger;
        
        private bool _isActivate;
        private LocationsManager _locationsManager;

        [Inject]
        private void Construct(LocationsManager locationsManager)
        {
            _locationsManager = locationsManager;
        }
        
        public void Use()
        {
            if (_isActivate)
            {
                _locationsManager.SwitchLocation("MyCell", 0);
            }
            else
            {
                _dialogueSystemTrigger.OnUse();
            }
        }

        public void Activate()
        {
            _isActivate = true;
        }
    }
}