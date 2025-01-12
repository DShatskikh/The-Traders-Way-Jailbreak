using System;
using System.Collections;
using DG.Tweening;
using PixelCrushers.DialogueSystem;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Game
{
    public class SirenCutscene : MonoBehaviour
    {
        [SerializeField]
        private DialogueSystemTrigger _startDialogue;

        [SerializeField]
        private Image _image;

        [SerializeField]
        private string[] _allId;

        [SerializeField]
        private UnityEvent _dontPayTaxEvent;
        
        private SaveData _saveData;
        private WalletService _walletService;

        [Serializable]
        public struct SaveData
        {
            public State State;
        }

        public enum State
        {
            OFF,
            DontPayTax,
            PayTax,
            EndSpeakMayor
        }

        [Inject]
        private void Construct(WalletService walletService)
        {
            _walletService = walletService;
        }
        
        private void Start()
        {
            _saveData = CutscenesDataStorage.GetData<SaveData>(KeyConstants.Siren);
            UpgradeState(_saveData.State);
            //UpgradeState(State.DontPayTax);
        }

        private void OnDestroy()
        {
            BuyPlate.BuyEvent -= OnBuy;
            _walletService.TaxChanged -= WalletServiceOnTaxChanged;
        }

        private void OnBuy()
        {
            bool isAllBuy = true;
            int countBuy = 0;
            
            foreach (var id in _allId)
            {
                var data = CutscenesDataStorage.GetData<BuyPlate.SaveData>(id);

                if (!data.IsBuy)
                {
                    isAllBuy = false;
                    continue;
                }
                else
                {
                    countBuy += 1;
                }
            }

            if (isAllBuy)
            {
                _startDialogue.OnUse();
                UpgradeState(State.DontPayTax);
            }

            print($"Куплено {countBuy} из {_allId.Length}");
        }

        public void UpgradeState(State state)
        {
            switch (state)
            {
                case State.OFF:
                    BuyPlate.BuyEvent += OnBuy;
                    break;
                case State.DontPayTax:
                    DialogueExtensions.SubscriptionCloseDialog(() =>
                    {
                        StartCoroutine(AwaitDontPayTax());
                    });
                    
                    if (_walletService.GetTax == 0)
                        _walletService.SetMoneyAndTax((int)_walletService.GetMoney, 1);
                    
                    _walletService.TaxChanged += WalletServiceOnTaxChanged;
                    _dontPayTaxEvent.Invoke();
                    break;
                case State.PayTax:
                    StartCoroutine(AwaitPayTax());
                    break;
                case State.EndSpeakMayor:
                    print("SpeakMayor");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            _saveData.State = state;
            CutscenesDataStorage.SetData(KeyConstants.Siren, _saveData);
        }

        private void WalletServiceOnTaxChanged(double value)
        {
            if (value == 0)
            {
                _walletService.TaxChanged -= WalletServiceOnTaxChanged;
                UpgradeState(State.PayTax);
            }
        }

        private IEnumerator AwaitDontPayTax()
        {
            _image.gameObject.SetActive(true);

            while (true)
            {
                var sequence = DOTween.Sequence();
                yield return sequence
                    .Append(_image.DOColor(_image.color.SetA(0.2f), 1))
                    .Append(_image.DOColor(_image.color.SetA(0f), 1))
                    .WaitForCompletion();

                yield return new WaitForSeconds(1f);
            }
        }

        private IEnumerator AwaitPayTax()
        {
            print("AwaitPayTax");
            yield return null;
            UpgradeState(State.EndSpeakMayor);
        }
    }
}