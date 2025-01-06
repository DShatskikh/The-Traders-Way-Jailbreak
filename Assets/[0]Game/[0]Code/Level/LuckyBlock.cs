using System.Collections;
using PixelCrushers.DialogueSystem;
using UnityEngine;

namespace Game
{
    public class LuckyBlock : MonoBehaviour, IUseObject
    {
        [SerializeField]
        private DialogueSystemTrigger _addDialogueSystemTrigger, _dialogueSystemTrigger;

        [SerializeField]
        private Sprite[] _breacks;

        [SerializeField]
        private ParticleSystem _effect;
        
        private StockMarketService _stockMarketService;
        private SpriteRenderer _spriteRenderer;
        private Player _player;
        private GameStateController _gameStateController;

        [Inject]
        private void Construct(StockMarketService stockMarketService, Player player, GameStateController gameStateController)
        {
            _stockMarketService = stockMarketService;
            _player = player;
            _gameStateController = gameStateController;
        }

        private void Start()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            
            if (_stockMarketService.IsOpenItem("LuckyBlock"))
                gameObject.SetActive(false);
        }

        public void Use()
        {
            StartCoroutine(AwaitAdd());
        }

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
            
            _stockMarketService.AddItem("LuckyBlock");
            gameObject.SetActive(false);
        }
    }
}