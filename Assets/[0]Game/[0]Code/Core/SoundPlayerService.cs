using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public sealed class SoundPlayerService
    {
        [SerializeField]
        private AudioSource _audioSource;
        public AudioClip Clip => _audioSource.clip;

        public void Play(AudioClip clip)
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