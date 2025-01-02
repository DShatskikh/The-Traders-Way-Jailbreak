using System;
using PixelCrushers;
using PixelCrushers.DialogueSystem;
using UnityEngine;
using UnityEngine.Events;

namespace Game
{
    public class HomeCutscene : MonoBehaviour, IGameLaptopListener
    {
        [SerializeField]
        private DialogueSystemTrigger _startDialogue, _closeLaptopDialogue, _tvNews, _tv;

        [SerializeField]
        private TargetArrow _laptopArrow, _tvArrow, _bedArrow;

        [SerializeField]
        private GameObject _nightPanel;

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

        private void Start()
        {
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

        private void UpgradeState(CutsceneState transitionState)
        {
            switch (transitionState)
            {
                case CutsceneState.LAPTOP when _cutsceneState != CutsceneState.OFF:
                    return;
                case CutsceneState.LAPTOP:
                    _startDialogue.OnUse();
                    _laptopArrow.gameObject.SetActive(true);
                    _cutsceneState = CutsceneState.LAPTOP;
                    break;
                case CutsceneState.TV when _cutsceneState != CutsceneState.LAPTOP:
                    return;
                case CutsceneState.TV:
                    _laptopArrow.gameObject.SetActive(false);
                    _tvArrow.gameObject.SetActive(true);
                    _cutsceneState = CutsceneState.TV;
                    _closeLaptopDialogue.OnUse();
                    _tvNews.gameObject.SetActive(true);
                    _tv.gameObject.SetActive(false);
                    break;
                case CutsceneState.BED when _cutsceneState != CutsceneState.TV:
                    return;
                case CutsceneState.BED:
                    _tvArrow.gameObject.SetActive(false);
                    _bedArrow.gameObject.SetActive(true);
                    _cutsceneState = CutsceneState.BED;
                    break;
                case CutsceneState.POLICE when _cutsceneState != CutsceneState.BED:
                    return;
                case CutsceneState.POLICE:
                    _bedArrow.gameObject.SetActive(false);
                    _nightPanel.SetActive(true);
                    _cutsceneState = CutsceneState.BED;
                    MusicPlayer.Play(_policeClip);
                    _policeEvent.Invoke();
                    
                    CutscenesDataStorage.SetData("HomeCutscene", new SaveData()
                    {
                        CutsceneState = CutsceneState.POLICE
                    });
                    break;
            }
        }
    }
}