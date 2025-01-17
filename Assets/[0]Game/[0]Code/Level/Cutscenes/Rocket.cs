using System.Collections;
using DG.Tweening;
using PixelCrushers.DialogueSystem;
using UnityEngine;

namespace Game
{
    public sealed class Rocket : MonoBehaviour, IUseObject
    {
        [SerializeField]
        private DialogueSystemTrigger _dialogueSystemTrigger;

        [SerializeField]
        private GameObject _player, _chief, _dilleron, _noobik, _effect, _arrow;
        
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
                StartCoroutine(AwaitTakeOff());
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

        private IEnumerator AwaitTakeOff()
        {
            _arrow.SetActive(false);
            
            ServiceLocator.Get<Player>().gameObject.SetActive(false);
            _player.SetActive(true);
            yield return new WaitForSeconds(1f);
            
            ServiceLocator.Get<CompanionsManager>().TryDeactivateCompanion("PoliceChief");
            _chief.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            
            ServiceLocator.Get<CompanionsManager>().TryDeactivateCompanion("PoliceDilleron");
            _dilleron.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            
            ServiceLocator.Get<CompanionsManager>().TryDeactivateCompanion("PoliceNoobik");
            _noobik.SetActive(true);
            yield return new WaitForSeconds(1f);

            _effect.SetActive(true);
            yield return new WaitForSeconds(1f);
            
            var sequence = DOTween.Sequence();
            yield return sequence.Append(transform.DOMoveY(transform.position.AddY(10).y, 3f)).WaitForCompletion();
            
            _locationsManager.SwitchLocation("MyCell", 0);
        }
    }
}