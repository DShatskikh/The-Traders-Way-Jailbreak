using System;
using System.Collections;
using PixelCrushers.DialogueSystem;
using UnityEngine;

namespace Game
{
    public abstract class BaseLuckyBlock : MonoBehaviour, IUseObject
    {
        [SerializeField]
        private DialogueSystemTrigger _addDialogueSystemTrigger, _dialogueSystemTrigger;

        [SerializeField]
        private Sprite[] _breacks;

        [SerializeField]
        private ParticleSystem _effect;
        
        protected StockMarketService _stockMarketService;
        protected Player _player;
        protected GameStateController _gameStateController;
        private SpriteRenderer _spriteRenderer;

        public abstract bool CanMining { get; }

        private void Awake()
        {
            _stockMarketService = ServiceLocator.Get<StockMarketService>();
            _player = ServiceLocator.Get<Player>();
            _gameStateController = ServiceLocator.Get<GameStateController>();
            
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Start()
        {
            gameObject.SetActive(CanMining);
        }

        public void Use()
        {
            StartCoroutine(AwaitAdd());
        }

        public abstract void AddPrize();
        
        private IEnumerator AwaitAdd()
        {
            _dialogueSystemTrigger.OnUse();
            yield return DialogueExtensions.AwaitCloseDialog();
            _gameStateController.OpenDialog();
            _player.SetViewState(2);
            
            yield return new WaitForSeconds(1);

            foreach (var sprite in _breacks)
            {
                _spriteRenderer.sprite = sprite;
                yield return new WaitForSeconds(1f);
            }

            _player.SetViewState(0);
            _spriteRenderer.enabled = false;
            _effect.Play();
            
            yield return new WaitForSeconds(0.5f);
            
            _addDialogueSystemTrigger.OnUse();
            yield return DialogueExtensions.AwaitCloseDialog();

            AddPrize();
            gameObject.SetActive(false);
        }
    }
}