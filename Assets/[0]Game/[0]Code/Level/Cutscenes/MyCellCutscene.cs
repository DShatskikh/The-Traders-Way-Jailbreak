using System;
using System.Collections;
using DG.Tweening;
using PixelCrushers.DialogueSystem;
using UnityEngine;

namespace Game
{
    public sealed class MyCellCutscene : MonoBehaviour, IGameLaptopListener
    {
        [SerializeField]
        private ItNightScreen _screen;

        [SerializeField]
        private DialogueSystemTrigger _dialogue;
        
        [SerializeField]
        private DialogueSystemTrigger _dontPayDialogue;
        
        [SerializeField]
        private DialogueSystemTrigger _payDialogue;

        [SerializeField]
        private GameObject _mayor;
        
        private SaveData _saveData;
        private Player _player;
        private WalletService _walletService;
        private GameStateController _gameStateController;

        [Serializable]
        public struct SaveData
        {
            public bool IsNotFirstVisit;
            public State State;
        }

        public enum State
        {
            OFF, //Игрок все не скупил
            DontPayTax, //Игрок все скупил
            PayTax, //Игрок оплатил налог
            EndSpeakMayor //Игрок поговорил с мэром
        }
        
        [Inject]
        private void Construct(Player player, WalletService walletService, GameStateController gameStateController)
        {
            _player = player;
            _walletService = walletService;
            _gameStateController = gameStateController;
        }
        
        private void Start()
        {
            _saveData = RepositoryStorage.Get<SaveData>(KeyConstants.MyCellCutscene);
            
            if (!_saveData.IsNotFirstVisit)
            {
                _saveData.IsNotFirstVisit = true;
                RepositoryStorage.Set(KeyConstants.MyCellCutscene, _saveData);
                StartCoroutine(AwaitCutscene());
            }

            if (_saveData.State == State.DontPayTax)
            {
                _dontPayDialogue.OnUse();
            }
        }

        private IEnumerator AwaitCutscene()
        {
            var tax = _walletService.GetTax - _walletService.GetMoney;

            if (tax < 0)
                tax = 0;
            
            _walletService.SetMoneyAndTax(4, tax);
            _player.gameObject.SetActive(true);
            _gameStateController.OpenDialog();
            _screen.gameObject.SetActive(true);
            yield return _screen.AwaitAnimation();
            _dialogue.OnUse();
        }

        public void OnOpenLaptop()
        {
            
        }

        public void OnCloseLaptop()
        {
            _saveData = RepositoryStorage.Get<SaveData>(KeyConstants.MyCellCutscene);

            if (_saveData.State == State.DontPayTax)
            {
                _saveData.State = State.PayTax;
                _mayor.SetActive(true);
                _payDialogue.OnUse();

                DialogueExtensions.SubscriptionCloseDialog(() =>
                {
                    _saveData.State = State.EndSpeakMayor;
                    
                    var sequence = DOTween.Sequence();
                    sequence.Append(_mayor.transform.DOMoveY(_mayor.transform.position.y + 15, 3))
                        .SetEase(Ease.Linear)
                        .OnComplete(() => { _mayor.SetActive(false);});
                    RepositoryStorage.Set(KeyConstants.MyCellCutscene, _saveData);
                });
            }
        }
    }
}