using System;
using System.Collections;
using PixelCrushers.DialogueSystem;
using UnityEngine;

namespace Game
{
    public class MyCellCutscene : MonoBehaviour
    {
        [SerializeField]
        private GameObject _screen;

        [SerializeField]
        private DialogueSystemTrigger _dialogue;
        
        private SaveData _saveData;
        private SirenCutscene.SaveData _sirenSaveData;
        private Player _player;
        private WalletService _walletService;

        [Serializable]
        public struct SaveData
        {
            public bool IsNotFirstVisit;
        }

        [Inject]
        private void Construct(Player player, WalletService walletService)
        {
            _player = player;
            _walletService = walletService;
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
            _screen.SetActive(true);
            _walletService.Add(4);
            yield return new WaitForSeconds(3f);
            _player.gameObject.SetActive(true);
            _dialogue.OnUse();
        }
    }
}