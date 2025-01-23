using System;
using UnityEngine;
using UnityEngine.Audio;

namespace Game
{
    [Serializable]
    public class VolumeService : IDisposable
    {
        [SerializeField]
        private AudioMixer _audioMixer;

        public ReactiveProperty<float> Volume = new();

        public void Init()
        {
            Volume.Changed += OnChangeVolume;
            Volume.Value = RepositoryStorage.Get<VolumeData>(KeyConstants.Volume).Volume;
        }

        public void Dispose()
        {
            Volume.Changed -= OnChangeVolume;
        }

        private void OnChangeVolume(float volume)
        {
            _audioMixer.SetFloat("MasterVolume", Mathf.Lerp(-80, 0, volume));
        }
    }
}