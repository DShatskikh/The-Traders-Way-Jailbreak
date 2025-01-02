using System;
using QFSW.QC;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game
{
    [Serializable]
    public class ConsoleService
    {
        [SerializeField]
        private QuantumConsole _quantumConsole;

        private PlayerInput _playerInput;
        private WalletService _walletService;
        private static ConsoleService _instance;

        [Inject]
        private void Construct(PlayerInput playerInput, WalletService walletService)
        {
            _playerInput = playerInput;
            _walletService = walletService;
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
    }
}