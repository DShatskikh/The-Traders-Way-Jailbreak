using System.Collections;
using DG.Tweening;
using PixelCrushers.DialogueSystem.Wrappers;
using UnityEngine;

namespace Game
{
    public sealed class HangarRocket : MonoBehaviour, IUseObject
    {
        [SerializeField]
        private DialogueSystemTrigger _endGameDialog;
        
        [SerializeField]
        private DialogueSystemTrigger _defaultDialog;

        [SerializeField]
        private GameObject _fakePlayer;

        [SerializeField]
        private GameObject _effect;
        
        private LocationsManager _levelManager;
        private Player _player;
        private GameStateController _gameStateController;

        [Inject]
        private void Construct(LocationsManager levelManager, Player player, GameStateController gameStateController)
        {
            _levelManager = levelManager;
            _player = player;
            _gameStateController = gameStateController;
        }
        
        public void Use()
        {
            if (RepositoryStorage.Get<MyCellCutscene.SaveData>(KeyConstants.MyCellCutscene).State ==
                MyCellCutscene.State.EndSpeakMayor)
            {
                _endGameDialog.OnUse();
            }
            else
            {
                _defaultDialog.OnUse();
            }
        }

        public void EndGame()
        {
            DialogueExtensions.SubscriptionCloseDialog(() =>
            {
                print("EndGame");
                StartCoroutine(AwaitEndGame());
            });
        }

        private IEnumerator AwaitEndGame()
        {
            _gameStateController.OpenDialog();
            _player.gameObject.SetActive(false);
            _fakePlayer.SetActive(true);
            
            yield return new WaitForSeconds(0.5f);
            
            _effect.SetActive(true);
            yield return new WaitForSeconds(1f);
            
            var sequence = DOTween.Sequence();
            yield return sequence.Append(transform.DOMoveY(transform.position.AddY(10).y, 3f)).WaitForCompletion();
            
            var homeCutsceneSaveData = RepositoryStorage.Get<HomeCutscene.SaveData>(KeyConstants.HomeCutscene);
            homeCutsceneSaveData.CutsceneState = HomeCutscene.CutsceneState.ENDING;
            RepositoryStorage.Set(KeyConstants.HomeCutscene, homeCutsceneSaveData);
            
            _player.gameObject.SetActive(true);
            _gameStateController.CloseDialog();
            _levelManager.SwitchLocation("WorldHome", 3);
        }
    }
}