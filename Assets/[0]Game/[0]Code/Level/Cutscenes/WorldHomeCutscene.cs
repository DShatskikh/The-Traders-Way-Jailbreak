using System.Collections;
using System.Collections.Generic;
using PixelCrushers.DialogueSystem;
using UnityEngine;
using YG;

namespace Game
{
    public sealed class WorldHomeCutscene : MonoBehaviour
    {
        [SerializeField]
        private GameObject _root;

        [SerializeField]
        private TargetArrow _rocketArrow;

        [SerializeField]
        private DialogueSystemTrigger _startDialogue;

        [SerializeField]
        private Rocket _rocket;

        [SerializeField]
        private TriggerChecker _followingTrigger;

        [SerializeField]
        private GameObject _chief, _noobik, _dilleron, _endGame, _table, _leftTransition, _rightTransition;

        [SerializeField]
        private TransitionTrigger _transitionTrigger;
        
        [Header("Secret")]
        [SerializeField]
        private DialogueSystemTrigger _secretEndingDialogue;

        [SerializeField]
        private HackerStatue _hackerStatue;
        
        private StockMarketService _stockMarketService;
        private WalletService _walletService;
        private HomeCutscene.SaveData _data;
        private CompanionsManager _companionsManager;
        private Player _player;
        private HatManager _hatManager;

        [Inject]
        private void Construct(StockMarketService stockMarketService, WalletService walletService,
            CompanionsManager companionsManager, Player player, HatManager hatManager)
        {
            _stockMarketService = stockMarketService;
            _walletService = walletService;
            _companionsManager = companionsManager;
            _player = player;
            _hatManager = hatManager;
        }
        
        private void Start()
        {
            _data = RepositoryStorage.Get<HomeCutscene.SaveData>(KeyConstants.HomeCutscene);

            if (_data.CutsceneState == HomeCutscene.CutsceneState.POLICE)
            {
                if (_walletService.GetTax == 0)
                {
                    _data.CutsceneState = HomeCutscene.CutsceneState.SECRETENDING;
                    RepositoryStorage.Set(KeyConstants.HomeCutscene, _data);
                }
                else
                    StartCoroutine(AwaitPoliceCutscene());
            }

            if (_data.CutsceneState == HomeCutscene.CutsceneState.SECRETENDING)
            {
                _secretEndingDialogue.OnUse();
                
                DialogueExtensions.SubscriptionCloseDialog(() =>
                {
                    _transitionTrigger.GetComponent<BoxCollider2D>().isTrigger = false;
                    _hackerStatue.ShowArrow();
                    print("SECRETENDING");
                });
            }

            if (_data.CutsceneState is HomeCutscene.CutsceneState.ENDING or HomeCutscene.CutsceneState.PARTY)
            {
                _player.gameObject.SetActive(true);   
                _endGame.SetActive(true);
                _table.SetActive(true);
                _transitionTrigger.GetComponent<BoxCollider2D>().isTrigger = false;
                _leftTransition.SetActive(false);
                _rightTransition.SetActive(false);
            }
        }

        private IEnumerator AwaitPoliceCutscene()
        {
            //_followingTrigger.TriggerEnter += TriggerEnter;
            _transitionTrigger.GetComponent<BoxCollider2D>().isTrigger = false;
            
            _stockMarketService.ResetToDefault();
            _root.SetActive(true);
            _startDialogue.OnUse();
            yield return null;
            
            DialogueExtensions.SubscriptionCloseDialog(() =>
            {
                _rocketArrow.gameObject.SetActive(true);
                _rocket.Activate();
                
                _followingTrigger.TriggerEnter += TriggerEnter;
            });
        }

        private void TriggerEnter(GameObject obj)
        {
            if (obj.TryGetComponent(out Player player))
            {
                _followingTrigger.TriggerEnter -= TriggerEnter;

                if (_companionsManager.TryActivateCompanion("PoliceChief", _chief.transform.position, 
                        out Companion chief, _chief.GetComponent<SpriteRenderer>().flipX))
                {
                    _chief.SetActive(false);
                }
                
                if (_companionsManager.TryActivateCompanion("PoliceNoobik", _noobik.transform.position, 
                        out Companion noobik, _noobik.GetComponent<SpriteRenderer>().flipX))
                {
                    _noobik.SetActive(false);
                }
                
                if (_companionsManager.TryActivateCompanion("PoliceDilleron", _dilleron.transform.position, 
                        out Companion dilleron, _dilleron.GetComponent<SpriteRenderer>().flipX))
                {
                    _dilleron.SetActive(false);
                }
            }
        }
    }
}