using System;
using System.Collections;
using PixelCrushers.DialogueSystem;
using UnityEngine;
using UnityEngine.Events;

namespace Game
{
    public class HomeCutscene : MonoBehaviour, IGameLaptopListener, IGameTransitionListener, IGameStartListener
    {
        [SerializeField]
        private DialogueSystemTrigger _startDialogue, _closeLaptopDialogue, _tvNews, _tv;

        [SerializeField]
        private TargetArrow _laptopArrow, _tvArrow, _bedArrow;

        [SerializeField]
        private GameObject _nightPanel;

        [SerializeField]
        private ItNightScreen _policePanel;
        
        [SerializeField]
        private AudioClip _policeClip;

        [SerializeField]
        private UnityEvent _policeEvent;
        
        private StockMarketService _stockMarketService;
        private CutsceneState _cutsceneState;
        private WalletService _walletService;

        [Serializable]
        public enum CutsceneState
        {
            OFF = 0,
            LAPTOP = 1, //Когда нужно зайти на биржу
            TV = 2, //Посмотреть телевизор
            BED = 3, //Лечь спать
            POLICE = 4, //Ломятся полицейские
            EndGame = 5, //Игрок прилетел домой
            Party = 6, //Игрок Поговорил с разработчиком
        }
        
        [Serializable]
        public struct SaveData
        {
            public CutsceneState CutsceneState;
        }
        
        [Inject]
        private void Construct(StockMarketService stockMarketService, WalletService walletService)
        {
            _stockMarketService = stockMarketService;
            _walletService = walletService;
        }

        public void OnStartGame()
        {
            var data = CutscenesDataStorage.GetData<SaveData>(KeyConstants.HomeCutscene);
            
            if (data.CutsceneState == CutsceneState.EndGame)
                return;
                
            StartCoroutine(AwaitUpgradeState(CutsceneState.LAPTOP));
        }
        
        public void OnStartTransition()
        {
            
        }

        public void OnEndTransition()
        {
            var data = CutscenesDataStorage.GetData<HomeCutscene.SaveData>("HomeCutscene");
            
            if (data.CutsceneState == CutsceneState.EndGame)
                return;
            
            UpgradeState(CutsceneState.LAPTOP);
        }

        public void OffTV()
        {
            UpgradeState(CutsceneState.BED);
        }

        public void Sleep()
        {
            UpgradeState(CutsceneState.POLICE);
        }

        public void OnOpenLaptop()
        {
            
        }

        public void OnCloseLaptop()
        {
            UpgradeState(CutsceneState.TV);
        }

        private IEnumerator AwaitUpgradeState(CutsceneState transitionState)
        {
            yield return null;
            UpgradeState(transitionState);
        }

        private void UpgradeState(CutsceneState transitionState)
        {
            switch (transitionState)
            {
                case CutsceneState.LAPTOP when _cutsceneState != CutsceneState.OFF:
                    return;
                case CutsceneState.LAPTOP:
                    _startDialogue.OnUse();
                    DialogueExtensions.SubscriptionCloseDialog(() =>
                    {
                        _laptopArrow.gameObject.SetActive(true);
                    });
                    
                    _cutsceneState = CutsceneState.LAPTOP;
                    break;
                case CutsceneState.TV when _cutsceneState != CutsceneState.LAPTOP:
                    return;
                case CutsceneState.TV:
                    _closeLaptopDialogue.OnUse();
                    DialogueExtensions.SubscriptionCloseDialog(() =>
                    {
                        _tvArrow.gameObject.SetActive(true);
                        _tvNews.gameObject.SetActive(true);
                    });
                    
                    _laptopArrow.gameObject.SetActive(false);
                    _tv.gameObject.SetActive(false);
                    _cutsceneState = CutsceneState.TV;
                    break;
                case CutsceneState.BED when _cutsceneState != CutsceneState.TV:
                    return;
                case CutsceneState.BED:
                    DialogueExtensions.SubscriptionCloseDialog(() =>
                    {
                        _bedArrow.gameObject.SetActive(true);
                    });
                    
                    _tvArrow.gameObject.SetActive(false);
                    _cutsceneState = CutsceneState.BED;
                    
                    CutscenesDataStorage.SetData(KeyConstants.HomeCutscene, new SaveData()
                    {
                        CutsceneState = CutsceneState.BED
                    });
                    break;
                case CutsceneState.POLICE when _cutsceneState != CutsceneState.BED:
                    return;
                case CutsceneState.POLICE:
                    _bedArrow.gameObject.SetActive(false);
                    _nightPanel.SetActive(true);
                    MusicPlayer.Play(_policeClip);
                    _policeEvent.Invoke();
                    _cutsceneState = CutsceneState.BED;

                    _policePanel.gameObject.SetActive(true);
                    StartCoroutine(_policePanel.AwaitAnimation());
                    
                    CutscenesDataStorage.SetData(KeyConstants.HomeCutscene, new SaveData()
                    {
                        CutsceneState = CutsceneState.POLICE
                    });
                    break;
            }
        }
    }
}