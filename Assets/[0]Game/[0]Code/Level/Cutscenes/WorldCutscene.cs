using System.Collections;
using PixelCrushers.DialogueSystem;
using UnityEngine;

namespace Game
{
    public class WorldCutscene : MonoBehaviour
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
        private GameObject _chief, _noobik, _dilleron;
        
        private StockMarketService _stockMarketService;
        private WalletService _walletService;
        private HomeCutscene.SaveData _data;
        private CompanionsManager _companionsManager;
        private Player _player;

        [Inject]
        private void Construct(StockMarketService stockMarketService, WalletService walletService, CompanionsManager companionsManager, Player player)
        {
            _stockMarketService = stockMarketService;
            _walletService = walletService;
            _companionsManager = companionsManager;
            _player = player;
        }
        
        private void Start()
        {
            Lua.RegisterFunction(nameof(IsWorldPolice), this,
                SymbolExtensions.GetMethodInfo(() => IsWorldPolice()));
            
            _data = CutscenesDataStorage.GetData<HomeCutscene.SaveData>("HomeCutscene");

            if (_data.CutsceneState == HomeCutscene.CutsceneState.POLICE)
            {
                StartCoroutine(AwaitCutscene());
            }
            
            if (_data.CutsceneState == HomeCutscene.CutsceneState.EndGame)
                _player.gameObject.SetActive(true);
        }

        private IEnumerator AwaitCutscene()
        {
            //_followingTrigger.TriggerEnter += TriggerEnter;
            
            _stockMarketService.ResetToDefault();
            _root.SetActive(true);
            _startDialogue.OnUse();
            _walletService.Reset();
            yield return null;
            
            DialogueExtensions.SubscriptionCloseDialog(() =>
            {
                _rocketArrow.gameObject.SetActive(true);
                _rocket.Activate();
                
                _followingTrigger.TriggerEnter += TriggerEnter;
            });
        }

        private bool IsWorldPolice() => 
            _data.CutsceneState == HomeCutscene.CutsceneState.POLICE;

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