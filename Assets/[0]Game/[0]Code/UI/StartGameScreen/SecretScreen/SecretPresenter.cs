using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game
{
    public interface ISecretPresenter
    {
        void Show();
        void Hide();
        void Select(ISecretSlotPresenter secretSlotPresenter);
    }
    
    public sealed class SecretPresenter : ISecretPresenter
    {
        private readonly StartGameScreen _startGameScreen;
        private readonly SecretSlotView _prefab;
        private readonly ISecretSlotData[] _configs;
        private readonly SecretView _view;
        
        private List<ISecretSlotPresenter> _slots = new();
        private ISecretSlotPresenter _selectSlot;
        private readonly PlayerInput _playerInput;

        public SecretPresenter(SecretView view, ISecretSlotData[] configs, SecretSlotView prefab, 
            StartGameScreen startGameScreen, PlayerInput playerInput)
        {
            _view = view;
            _configs = configs;
            _prefab = prefab;
            _startGameScreen = startGameScreen;
            _playerInput = playerInput;
        }

        public void Show()
        {
            _view.Show();
            _view.ExitButton.onClick.AddListener(OnExitButtonClicked);

            foreach (var config in _configs)
            {
                var slot = new SecretSlotPresenter(config, _prefab, _view.Container, this);
                _slots.Add(slot);
            }

            _selectSlot = _slots[0];
            _selectSlot.Select();
            UpgradeView();
            
            _playerInput.actions["Move"].started += OnMove;
        }

        public void Hide()
        {
            _playerInput.actions["Move"].started -= OnMove;
            _view.ExitButton.onClick.RemoveListener(OnExitButtonClicked);

            foreach (var slot in _slots) 
                slot.Dispose();

            _slots = new List<ISecretSlotPresenter>();
            _view.Hide();
        }

        public void Select(ISecretSlotPresenter secretSlotPresenter)
        {
            foreach (var slot in _slots) 
                slot.Deselect();
            
            secretSlotPresenter.Select();
            _selectSlot = secretSlotPresenter;
            UpgradeView();
        }

        private void OnExitButtonClicked()
        {
            Hide();
            _startGameScreen.gameObject.SetActive(true);
        }

        private void UpgradeView()
        {
            var config = _selectSlot.GetConfig;
            _view.SetIcon(config.GetPicture);
            LocalizedTextUtility.Load(config.GetDescription, text => _view.SetDescription(text));
        }

        private void OnMove(InputAction.CallbackContext direction)
        {
            _selectSlot.Deselect();
            var index = _slots.IndexOf(_selectSlot);
            index -= (int)direction.ReadValue<Vector2>().y;
            index = Mathf.Clamp(index, 0, _slots.Count - 1);
            _selectSlot = _slots[index];
            _selectSlot.Select();
            UpgradeView();
        }
    }
}