using System;
using System.Collections;
using PixelCrushers.DialogueSystem;
using UnityEngine;

namespace Game
{
    public class MyCellCutscene : MonoBehaviour
    {
        [SerializeField]
        private ItNightScreen _screen;

        [SerializeField]
        private DialogueSystemTrigger _dialogue;
        
        private SaveData _saveData;
        private SirenCutscene.SaveData _sirenSaveData;
        private Player _player;
        private WalletService _walletService;
        private GameStateController _gameStateController;

        [Serializable]
        public struct SaveData
        {
            public bool IsNotFirstVisit;
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
            _saveData = CutscenesDataStorage.GetData<SaveData>("MyCellCutscene");
            
            if (!_saveData.IsNotFirstVisit)
            {
                _saveData.IsNotFirstVisit = true;
                CutscenesDataStorage.SetData("MyCellCutscene", _saveData);
                StartCoroutine(AwaitCutscene());
            }
            
            _sirenSaveData = CutscenesDataStorage.GetData<SirenCutscene.SaveData>("Siren");
            
            //if (_sirenSaveData.State == SirenCutscene.State.DontPayTax)
                
        }

        private IEnumerator AwaitCutscene()
        {
            _walletService.Add(4);
            _player.gameObject.SetActive(true);
            _gameStateController.OpenDialog();
            _screen.gameObject.SetActive(true);
            yield return _screen.AwaitAnimation();
            _dialogue.OnUse();
        }
    }
}