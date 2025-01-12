using PixelCrushers.DialogueSystem;
using UnityEngine;

namespace Game
{
    public class Bed : MonoBehaviour, IUseObject
    {
        [SerializeField]
        private HomeCutscene _homeCutscene;

        [SerializeField]
        private DialogueSystemTrigger _dialogTrigger;
        
        public void Use()
        {
            var data = CutscenesDataStorage.GetData<HomeCutscene.SaveData>(KeyConstants.HomeCutscene);

            if (data.CutsceneState == HomeCutscene.CutsceneState.BED)
            {
                _homeCutscene.Sleep();
            }
            else
            {
                _dialogTrigger.OnUse();
            }
        }
    }
}