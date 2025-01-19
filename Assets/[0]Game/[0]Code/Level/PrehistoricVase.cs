using System;
using System.Collections;
using PixelCrushers.DialogueSystem;
using UnityEngine;

namespace Game
{
    public sealed class PrehistoricVase : MonoBehaviour
    {
        [SerializeField]
        private SpriteRenderer _spriteRenderer;

        [SerializeField]
        private Sprite _replaceSprite;

        [SerializeField]
        private ParticleSystem _particleSystem;

        [SerializeField]
        private Collider2D _collider2D;
        
        private SaveData _saveData;
        private GameStateController _gameStateController;

        [Serializable]
        public struct SaveData
        {
            public bool IsBreak;
        }

        [Inject]
        private void Construct(GameStateController gameStateController)
        {
            _gameStateController = gameStateController;
        }
        
        private void Start()
        {
            if (RepositoryStorage.Get<SaveData>("Vase").IsBreak)
            {
                Replace();
            }
        }

        private void Break()
        {
            StartCoroutine(AwaitBreak());
        }

        private IEnumerator AwaitBreak()
        {
            var theme = MusicPlayer.Instance.Clip;
            var climTime = MusicPlayer.Instance.GetTime();
            MusicPlayer.Instance.Stop();
            Replace();
            _particleSystem.Play();
            _gameStateController.OpenDialog();
            yield return new WaitForSeconds(2);
            MusicPlayer.Play(theme, climTime);
            Sequencer.Message("\"EndBreak\"");
            RepositoryStorage.Set("Vase", new SaveData() {IsBreak = true});
        }

        private void Replace()
        {
            _spriteRenderer.sprite = _replaceSprite;
            _spriteRenderer.sortingOrder = -1;
            _collider2D.enabled = false;
        }
    }
}