using System;
using QFSW.QC;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Localization;

namespace Game
{
    [Serializable]
    public class ConsoleService
    {
        private static ConsoleService _instance;

        [SerializeField]
        private QuantumConsole _quantumConsole;

        [SerializeField]
        private LocalizedString _helpInfo;
        
        [SerializeField]
        private LocalizedString _secret;
        
        private PlayerInput _playerInput;
        private WalletService _walletService;
        private AllBuyCheckHandler _allBuyCheckHandler;
        private GameStateController _gameStateController;

        [Inject]
        private void Construct(PlayerInput playerInput, WalletService walletService,
            AllBuyCheckHandler allBuyCheckHandler, GameStateController gameStateController)
        {
            _playerInput = playerInput;
            _walletService = walletService;
            _allBuyCheckHandler = allBuyCheckHandler;
            _gameStateController = gameStateController;
        }

        public void Init()
        {
            _instance = this;
            
            _playerInput.actions["OpenConsole"].performed += Onperformed;
        }

        private void Onperformed(InputAction.CallbackContext obj)
        {
            _quantumConsole.gameObject.SetActive(!_quantumConsole.gameObject.activeSelf);
        }

        [Command()]
        public static void AddMoney(int money)
        {
            _instance._walletService.Add(money);
        }
        
        [Command()]
        public static void ResetTax()
        {
            _instance._walletService.SetMoneyAndTax(_instance._walletService.GetMoney, 0);
        }
        
        [Command()]
        public static void BuyAll()
        {
            _instance._allBuyCheckHandler.AllBuyTest();
        }
        
        [Command()]
        public static void Help()
        {
            LocalizedTextUtility.Load(_instance._helpInfo, Debug.Log);
        }
        
        [Command()]
        public static void Secret()
        {
            LocalizedTextUtility.Load(_instance._secret, Debug.Log);
        }
        
        [Command()]
        public static void GameStatus()
        {
            Debug.Log(_instance._gameStateController.CurrentState);
        }
        
        [Command()]
        public static void SetGameStatus(GameStateController.GameState status)
        {
            switch (status)
            {
                case GameStateController.GameState.OFF:
                    break;
                case GameStateController.GameState.PLAYING:
                    switch (_instance._gameStateController.CurrentState)
                    {
                        case GameStateController.GameState.TRANSITION:
                            _instance._gameStateController.EndTransition();
                            break;
                        case GameStateController.GameState.LAPTOP:
                            _instance._gameStateController.CloseLaptop();
                            break;
                        case GameStateController.GameState.SHOP:
                            _instance._gameStateController.CloseShop();
                            break;
                        case GameStateController.GameState.ADS:
                            _instance._gameStateController.CloseADS();
                            break;
                        case GameStateController.GameState.DIALOGUE:
                            _instance._gameStateController.CloseDialog();
                            break;
                        default:
                            _instance._gameStateController.ResumeGame();
                            break;
                    }
                    
                    break;
                case GameStateController.GameState.PAUSED:
                    _instance._gameStateController.PauseGame();
                    break;
                case GameStateController.GameState.MAIN_MENU:
                    _instance._gameStateController.OpenMainMenu();
                    break;
                case GameStateController.GameState.TRANSITION:
                    _instance._gameStateController.StartTransition();
                    break;
                case GameStateController.GameState.LAPTOP:
                    _instance._gameStateController.OpenLaptop();
                    break;
                case GameStateController.GameState.SHOP:
                    _instance._gameStateController.OpenShop();
                    break;
                case GameStateController.GameState.ADS:
                    _instance._gameStateController.OpenADS();
                    break;
                case GameStateController.GameState.DIALOGUE:
                    _instance._gameStateController.OpenDialog();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(status), status, null);
            }
        }
    }
}