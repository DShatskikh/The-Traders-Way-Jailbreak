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

        [Inject]
        private void Construct(StockMarketService stockMarketService, WalletService walletService)
        {
            _stockMarketService = stockMarketService;
            _walletService = walletService;
        }
        
        private void Start()
        {
            var data = CutscenesDataStorage.GetData<HomeCutscene.SaveData>();

            if (data.CutsceneState == HomeCutscene.CutsceneState.POLICE)
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
    }
}