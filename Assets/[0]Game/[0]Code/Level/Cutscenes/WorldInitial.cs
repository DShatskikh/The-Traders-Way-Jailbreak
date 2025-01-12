using PixelCrushers.DialogueSystem;
using UnityEngine;

namespace Game
{
    public class WorldInitial : MonoBehaviour
    {
        [SerializeField]
        private DialogueSystemTrigger _startMonolog;

        [SerializeField]
        private GameObject _defaultTransition, _secretTransition;
        
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
            var state = CutscenesDataStorage.GetData<HomeCutscene.SaveData>(KeyConstants.HomeCutscene).CutsceneState;
            
            if (state == HomeCutscene.CutsceneState.OFF)
            {
                _stockMarketService.OpenAllItems();
                _walletService.Add(13741646);
            }

            if (state == HomeCutscene.CutsceneState.EndGame)
            {
                _defaultTransition.SetActive(false);
                _secretTransition.SetActive(true);
            }

            //_startMonolog.OnUse();
        }
    }
}