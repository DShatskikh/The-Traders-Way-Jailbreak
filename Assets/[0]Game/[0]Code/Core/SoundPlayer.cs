using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public sealed class SoundPlayer
    {
        [SerializeField]
        private AudioSource _audioSource;
        
        public AudioClip Clip => _audioSource.clip;
        public static SoundPlayer Instance { get; private set; }

        public void Init()
        {
            Instance = this;
        }
        
        public static void Play(AudioClip clip)
        {
            Instance.PlayLocal(clip);
        }

        private void PlayLocal(AudioClip clip)
        {
            _audioSource.clip = clip;
            _audioSource.Play();
        }
        
        public void Stop()
        {
            _audioSource.Stop();
        }
    }
}