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
        
        [SerializeField]
        private AudioSource _audioSource_2;

        private bool _isFirst;
        
        public AudioClip Clip => _audioSource.clip;
        public static SoundPlayer Instance { get; private set; }

        public void Init()
        {
            Instance = this;
            
            Lua.RegisterFunction("PlayBuy", this,
                SymbolExtensions.GetMethodInfo(() => PlayBuy()));
            
            Lua.RegisterFunction("PlayBruh", this,
                SymbolExtensions.GetMethodInfo(() => PlayBruh()));
            
            Lua.RegisterFunction("PlayBreak", this,
                SymbolExtensions.GetMethodInfo(() => PlayBreak()));
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
            _audioSource_2.Stop();
        }

        private void PlayLocal(AudioClip clip)
        {
            if (_isFirst)
            {
                _audioSource.clip = clip;
                _audioSource.Play();  
            }
            else
            {
                _audioSource_2.clip = clip;
                _audioSource_2.Play();   
            }

            _isFirst = !_isFirst;
        }

        private void PlayBruh() => 
            PlayLocal(AssetProvider.Instance.BruhSound);
        
        private void PlayBuy() => 
            PlayLocal(AssetProvider.Instance.BuySound);
        
        private void PlayBreak() => 
            PlayLocal(AssetProvider.Instance.BreakSound);
    }
}