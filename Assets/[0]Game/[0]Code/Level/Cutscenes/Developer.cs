using PixelCrushers.DialogueSystem.Wrappers;
using UnityEngine;

namespace Game
{
    public sealed class Developer : MonoBehaviour, IUseObject
    {
        [SerializeField]
        private AudioClip _theme;

        [SerializeField]
        private DialogueSystemTrigger _dialogueSystemTrigger;

        [SerializeField]
        private GameObject _panel;
        
        private AudioClip _previousClip;
        private EndingsGame _endingsGame;

        [Inject]
        private void Construct(EndingsGame endingsGame)
        {
            _endingsGame = endingsGame;
        }
        
        public void Use()
        {
            _previousClip = MusicPlayer.Instance.Clip;
            MusicPlayer.Play(_theme);
            
            _dialogueSystemTrigger.OnUse();

            //var data = RepositoryStorage.Get<HomeCutscene.SaveData>(KeyConstants.HomeCutscene);
            
            DialogueExtensions.SubscriptionCloseDialog(() =>
            {
                MusicPlayer.Play(_previousClip);
                RepositoryStorage.Set(KeyConstants.HomeCutscene, new HomeCutscene.SaveData() {CutsceneState = HomeCutscene.CutsceneState.PARTY});
            });
        }

        public void ShowPanel()
        {
            _panel.SetActive(true);

            DialogueExtensions.SubscriptionCloseDialog(() =>
            {
                _panel.SetActive(false);
                _endingsGame.EndingGameStandard();
            });
        }
    }
}