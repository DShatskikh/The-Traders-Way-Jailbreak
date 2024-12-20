using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Game
{
    public class LaptopScreen : BaseScreen
    {
        [SerializeField]
        private StockMarketCell _cellPrefab;

        [SerializeField]
        private Transform _container;

        [SerializeField]
        private Button _payTax;

        [SerializeField]
        private Button _closeButton;
        
        private PlayerInput _playerInput;
        private GameStateController _gameStateController;
        private StockMarketService _stockMarketService;

        private void Awake()
        {
            _playerInput = ServiceLocator.Get<PlayerInput>();
            _stockMarketService = ServiceLocator.Get<StockMarketService>();
        }

        private void OnEnable()
        {
            _playerInput.actions["Cancel"].canceled += OnCancel;
        }

        private void OnDisable()
        {
            _playerInput.actions["Cancel"].canceled -= OnCancel;
        }

        private IEnumerator Start()
        {
            foreach (var item in _stockMarketService.GetSlots)
            {
                var cell = Instantiate(_cellPrefab, _container);
                yield return cell.Init(item);
            }
            
            _gameStateController = ServiceLocator.Get<GameStateController>();
            _closeButton.onClick.AddListener(() => OnCancel(new InputAction.CallbackContext()));
        }

        public void OnCancel(InputAction.CallbackContext obj)
        {
            _gameStateController.CloseLaptop();
        }
    }
}