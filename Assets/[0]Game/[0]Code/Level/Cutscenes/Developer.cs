using PixelCrushers.DialogueSystem.Wrappers;
using UnityEngine;

namespace Game
{
    public class Developer : MonoBehaviour, IUseObject
    {
        [SerializeField]
        private AudioClip _theme;

        [SerializeField]
        private DialogueSystemTrigger _dialogueSystemTrigger;
        
        private AudioClip _previousClip;

        public void Use()
        {
            _previousClip = MusicPlayer.Instance.Clip;
            MusicPlayer.Play(_theme);
            
            _dialogueSystemTrigger.OnUse();
            DialogueExtensions.SubscriptionCloseDialog(() =>
            {
                MusicPlayer.Play(_previousClip);
                CutscenesDataStorage.SetData(KeyConstants.HomeCutscene, new HomeCutscene.SaveData() {CutsceneState = HomeCutscene.CutsceneState.Party});
            });
        }
    }
}