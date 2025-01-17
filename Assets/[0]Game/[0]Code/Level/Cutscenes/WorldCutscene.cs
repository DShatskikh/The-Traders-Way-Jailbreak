using DG.Tweening;
using PixelCrushers.DialogueSystem;
using UnityEngine;

namespace Game
{
    public sealed class WorldCutscene : MonoBehaviour
    {
        private const double StartMoney = 137414948616; //137 миллиардов
        
        [SerializeField]
        private DialogueSystemTrigger _startMonolog;

        [SerializeField]
        private GameObject _defaultTransition, _secretTransition;

        [SerializeField]
        private CanvasGroup _canvasGroup;
        
        private StockMarketService _stockMarketService;
        private WalletService _walletService;
        private Sequence _sequrnce;
        private GameStateController _gameStateController;

        [Inject]
        private void Construct(StockMarketService stockMarketService, WalletService walletService, GameStateController gameStateController)
        {
            _stockMarketService = stockMarketService;
            _walletService = walletService;
            _gameStateController = gameStateController;
        }
        
        private void Start()
        {
            var state = RepositoryStorage.Get<HomeCutscene.SaveData>(KeyConstants.HomeCutscene).CutsceneState;
            
            if (state == HomeCutscene.CutsceneState.OFF)
            {
                _canvasGroup.gameObject.SetActive(true);
                _stockMarketService.OpenAllItems();
                _walletService.SetMoneyAndTax(StartMoney, StartMoney * 1.5f);

                _sequrnce?.Kill();
                _sequrnce = DOTween.Sequence();
                _gameStateController.OpenDialog();
                
                _sequrnce.Insert(5, _canvasGroup.DOFade(0, 1f)).OnComplete(() =>
                {
                    _canvasGroup.gameObject.SetActive(false);
                    _gameStateController.CloseDialog();
                });
            }

            if (state == HomeCutscene.CutsceneState.ENDING)
            {
                _defaultTransition.SetActive(false);
                _secretTransition.SetActive(true);
            }

            //_startMonolog.OnUse();
        }
    }
}