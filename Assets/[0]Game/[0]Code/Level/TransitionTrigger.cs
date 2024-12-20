using System.Collections;
using UnityEngine;

namespace Game
{
    public class TransitionTrigger : MonoBehaviour
    {
        [SerializeField]
        private string _nextLocationIndex = "World";

        [SerializeField]
        private int _pointIndex;
        
        [SerializeField]
        private AudioClip _audioClip;

        private CoroutineRunner _coroutineRunner;
        private TransitionScreen _transitionScreen;
        private LocationsManager _locationsManager;
        
        private void Awake()
        {
            _coroutineRunner = ServiceLocator.Get<CoroutineRunner>();
            _transitionScreen = ServiceLocator.Get<TransitionScreen>(); ;
            _locationsManager = ServiceLocator.Get<LocationsManager>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out Player player))
            {
                _coroutineRunner.StartCoroutine(AwaitTransition(player));
            }
        }

        private IEnumerator AwaitTransition(Player player)
        {
            player.enabled = false;
            yield return _transitionScreen.AwaitShow();
            
            if (_audioClip)
                SoundPlayer.Play(_audioClip);
            
            _locationsManager.SwitchLocation(_nextLocationIndex, _pointIndex);
            yield return _transitionScreen.AwaitHide();
            player.enabled = true;
        }
    }
}
