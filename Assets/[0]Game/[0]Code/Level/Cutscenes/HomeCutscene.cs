using System;
using System.Collections;
using PixelCrushers.DialogueSystem;
using UnityEngine;
using UnityEngine.Events;

namespace Game
{
    public sealed class HomeCutscene : MonoBehaviour, IGameLaptopListener
    {
        [SerializeField]
        private DialogueSystemTrigger _startDialogue, _closeLaptopDialogue, _tvNews, _tv;

        [SerializeField]
        private TargetArrow _laptopArrow, _tvArrow, _bedArrow;

        [Header("Police")]
        [SerializeField]
        private GameObject _nightPanel;

        [SerializeField]
        private ItNightScreen _policePanel;
        
        [SerializeField]
        private AudioClip _policeClip;

        [SerializeField]
        private BoxCollider2D _exitTransition;
        
        [SerializeField]
        private UnityEvent _policeEvent;
        
        [Header("Party")]
        [SerializeField]
        private UnityEvent _partyEvent;

        [SerializeField]
        private AudioClip _partyTheme;
        
        private CutsceneState _cutsceneState;

        [Serializable]
        public enum CutsceneState
        {
            OFF = 0,
            LAPTOP = 1, //Когда нужно зайти на биржу
            TV = 2, //Посмотреть телевизор
            BED = 3, //Лечь спать
            POLICE = 4, //Ломятся полицейские
            ENDING = 5, //Игрок прилетел домой
            PARTY = 6, //Игрок Поговорил с разработчиком
            SECRETENDING = 7 //Секретная концовка
        }
        
        [Serializable]
        public struct SaveData
        {
            public CutsceneState CutsceneState;
            public bool IsShowLandedScreen;
        }

        private void Start()
        {
            var data = RepositoryStorage.Get<SaveData>(KeyConstants.HomeCutscene);
            
            if (data.CutsceneState == CutsceneState.ENDING)
                return;

            if (data.CutsceneState == CutsceneState.OFF)
                data.CutsceneState = CutsceneState.LAPTOP;
            
            UpgradeState(data.CutsceneState);

            if (data.CutsceneState == CutsceneState.PARTY)
            {
                _partyEvent.Invoke();
                StartCoroutine(AwaitStartParty());
            }
        }

        public void OffTV()
        {
            TryUpgradeState(CutsceneState.BED);
        }

        public void Sleep()
        {
            TryUpgradeState(CutsceneState.POLICE);
        }

        public void OnOpenLaptop()
        {
            
        }

        public void OnCloseLaptop()
        {
            TryUpgradeState(CutsceneState.TV);
        }

        private IEnumerator AwaitUpgradeState(CutsceneState transitionState)
        {
            yield return null;
            TryUpgradeState(transitionState);
        }

        private bool CanSwitchState(CutsceneState transitionState)
        {
            switch (transitionState)
            {
                case CutsceneState.LAPTOP when _cutsceneState != CutsceneState.OFF:
                    return false;
                case CutsceneState.TV when _cutsceneState != CutsceneState.LAPTOP:
                    return false;
                case CutsceneState.BED when _cutsceneState != CutsceneState.TV:
                    return false;
                case CutsceneState.POLICE when _cutsceneState != CutsceneState.BED:
                    return false;
            }

            return true;
        }

        private void TryUpgradeState(CutsceneState transitionState)
        {
            if (!CanSwitchState(transitionState))
                return;

            UpgradeState(transitionState);
        }

        private void UpgradeState(CutsceneState transitionState)
        {
            switch (transitionState)
            {
                case CutsceneState.LAPTOP:
                    _exitTransition.isTrigger = false;
                    
                    _startDialogue.OnUse();
                    DialogueExtensions.SubscriptionCloseDialog(() =>
                    {
                        _laptopArrow.gameObject.SetActive(true);
                    });
                    
                    _cutsceneState = CutsceneState.LAPTOP;
                    break;
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
                case CutsceneState.BED:
                    DialogueExtensions.SubscriptionCloseDialog(() =>
                    {
                        _bedArrow.gameObject.SetActive(true);
                    });
                    
                    _tvArrow.gameObject.SetActive(false);
                    _tvNews.gameObject.SetActive(false);
                    _tv.gameObject.SetActive(true);
                    _cutsceneState = CutsceneState.BED;
                    
                    RepositoryStorage.Set(KeyConstants.HomeCutscene, new SaveData()
                    {
                        CutsceneState = CutsceneState.BED
                    });
                    break;
                case CutsceneState.POLICE:
                    _exitTransition.isTrigger = true;
                    _bedArrow.gameObject.SetActive(false);
                    _nightPanel.SetActive(true);
                    MusicPlayer.Play(_policeClip);
                    _policeEvent.Invoke();
                    _cutsceneState = CutsceneState.BED;

                    _policePanel.gameObject.SetActive(true);
                    StartCoroutine(_policePanel.AwaitAnimation());
                    
                    RepositoryStorage.Set(KeyConstants.HomeCutscene, new SaveData()
                    {
                        CutsceneState = CutsceneState.POLICE
                    });
                    break;
            }
        }

        private IEnumerator AwaitStartParty()
        {
            yield return null;
            MusicPlayer.Play(_partyTheme);
        }
    }
}