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
        
        private StockMarketService _stockMarketService;
        private WalletService _walletService;
        private HomeCutscene.SaveData _data;

        [Inject]
        private void Construct(StockMarketService stockMarketService, WalletService walletService)
        {
            _stockMarketService = stockMarketService;
            _walletService = walletService;
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
        }

        private IEnumerator AwaitCutscene()
        {
            _root.SetActive(true);
            _startDialogue.OnUse();
            yield return null;
            _rocketArrow.gameObject.SetActive(true);
            _rocket.Activate();
            _stockMarketService.ResetToDefault();
            _walletService.Reset();
        }

        private bool IsWorldPolice() => 
            _data.CutsceneState == HomeCutscene.CutsceneState.POLICE;
    }
}