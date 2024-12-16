using System;
using System.Collections;
using UnityEngine;

namespace Game
{
    public sealed class MusicPlayer : MonoBehaviour
    {
        [SerializeField]
        private AudioClip _music;

        private Coroutine _coroutine;
        private MusicPlayerService _musicPlayerService;

        private void Awake()
        {
            _musicPlayerService = DIContainer.Get<MusicPlayerService>();
        }

        private void OnEnable()
        {
            if (_coroutine != null) 
                StopCoroutine(_coroutine);
            
            _coroutine = StartCoroutine(AwaitPlay());
        }

        public void Play() => 
            _musicPlayerService.Play(_music);

        private IEnumerator AwaitPlay()
        {
            yield return null;
            Play();
        }
    }
}