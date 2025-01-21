using System;
using DG.Tweening;
using UnityEngine;

namespace Game
{
    public sealed class WorldCutscene : MonoBehaviour
    {
        private const double StartMoney = 13741494861; //137 миллиардов
        
        [SerializeField]
        private GameObject _defaultTransition, _secretTransition;

        [SerializeField]
        private CanvasGroup _canvasGroup;
        
        private StockMarketService _stockMarketService;
        private WalletService _walletService;
        private Sequence _sequrnce;
        private GameStateController _gameStateController;
        private Player _player;
        private ScreenManager _screenManager;

        [Inject]
        private void Construct(StockMarketService stockMarketService, WalletService walletService,
            GameStateController gameStateController, Player player, ScreenManager screenManager)
        {
            _stockMarketService = stockMarketService;
            _walletService = walletService;
            _gameStateController = gameStateController;
            _player = player;
            _screenManager = screenManager;
        }
        
        private void Start()
        {
            var state = RepositoryStorage.Get<HomeCutscene.SaveData>(KeyConstants.HomeCutscene).CutsceneState;
            
            if (state == HomeCutscene.CutsceneState.OFF)
            {
                _canvasGroup.gameObject.SetActive(true);
                _stockMarketService.OpenAllItems();
                _walletService.SetMoneyAndTax(StartMoney, StartMoney * 1.5f);

                _player.gameObject.SetActive(true);
                _screenManager.Show(ScreenType.Main);
                
                _sequrnce?.Kill();
                _sequrnce = DOTween.Sequence();
                //_gameStateController.StartGame();
                _gameStateController.OpenDialog();

                _sequrnce.Insert(5, _canvasGroup.DOFade(0, 1f)).OnComplete(() =>
                {
                    _canvasGroup.gameObject.SetActive(false);
                    _gameStateController.CloseDialog();
                });
            }

            if (state is HomeCutscene.CutsceneState.ENDING or HomeCutscene.CutsceneState.PARTY)
            {
                _defaultTransition.SetActive(false);
                _secretTransition.SetActive(true);
            }
        }

        private void OnDestroy()
        {
            _sequrnce?.Kill();
        }
    }
}