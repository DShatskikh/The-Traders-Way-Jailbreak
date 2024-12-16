using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public sealed class MusicPlayerService
    {
        [SerializeField]
        private AudioSource _audioSource;
        public AudioClip Clip => _audioSource.clip;

        public void Play(AudioClip clip, float time = 0)
        {
            if (_audioSource.clip == clip && _audioSource.isPlaying)
                return;

            _audioSource.clip = clip;

            if (time != 0)
                _audioSource.time = time;
            
            _audioSource.Play();
        }

        public void Stop()
        {
            _audioSource.Stop();
        }

        public float GetTime() => 
            _audioSource.time;
    }
}