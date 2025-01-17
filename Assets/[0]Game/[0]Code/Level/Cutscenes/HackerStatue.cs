using System.Collections;
using PixelCrushers.DialogueSystem;
using UnityEngine;

namespace Game
{
    public class HackerStatue : MonoBehaviour, IUseObject
    {
        [SerializeField]
        private DialogueSystemTrigger _defaultHackerDialogue;
        
        [SerializeField]
        private DialogueSystemTrigger _secretHackerDialogue;

        [SerializeField]
        private TargetArrow _maskArrow;

        [SerializeField]
        private GameObject _panel;

        [SerializeField]
        private AudioClip _secretSoundtrack;
        
        private HomeCutscene.SaveData _data;
        private EndingsGame _endingsGame;

        [Inject]
        private void Construct(EndingsGame endingsGame)
        {
            _endingsGame = endingsGame;
        }
        
        private IEnumerator Start()
        {
            yield return null;
            _data = RepositoryStorage.Get<HomeCutscene.SaveData>(KeyConstants.HomeCutscene);
        }

        public void Use()
        {
            if (_data.CutsceneState == HomeCutscene.CutsceneState.SECRETENDING)
            {
                _maskArrow.gameObject.SetActive(false);
                _secretHackerDialogue.OnUse();

                DialogueExtensions.SubscriptionCloseDialog(() =>
                {
                    _panel.SetActive(false);
                    _endingsGame.EndingGameSecret();
                });
            }
            else
            {
                _defaultHackerDialogue.OnUse();
            }
        }

        public void ShowPanel()
        {
            _panel.SetActive(true);
        }

        public void ShowArrow()
        {
            _maskArrow.gameObject.SetActive(true);
            MusicPlayer.Play(_secretSoundtrack);
        }
    }
}