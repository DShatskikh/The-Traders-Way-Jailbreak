using System;
using PixelCrushers.DialogueSystem;
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
            
            Lua.RegisterFunction("PlayBuy", this,
                SymbolExtensions.GetMethodInfo(() => PlayBuy()));
            
            Lua.RegisterFunction("PlayBruh", this,
                SymbolExtensions.GetMethodInfo(() => PlayBruh()));
        }

        public static void Play(AudioClip clip)
        {
            Instance.PlayLocal(clip);
        }

        public static void Stop()
        {
            Instance.StopLocal();
        }
        
        public void StopLocal()
        {
            _audioSource.Stop();
        }

        private void PlayLocal(AudioClip clip)
        {
            _audioSource.clip = clip;
            _audioSource.Play();
        }

        private void PlayBruh() => 
            PlayLocal(AssetProvider.Instance.BruhSound);
        
        private void PlayBuy() => 
            PlayLocal(AssetProvider.Instance.BuySound);
    }
}