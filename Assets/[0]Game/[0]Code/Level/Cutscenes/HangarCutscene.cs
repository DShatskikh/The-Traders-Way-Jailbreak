using UnityEngine;

namespace Game
{
    public sealed class HangarCutscene : MonoBehaviour
    {
        [SerializeField]
        private GameObject _mayor;
        
        private void Start()
        {
            var saveData = RepositoryStorage.Get<MyCellCutscene.SaveData>(KeyConstants.MyCellCutscene);

            if (saveData.State == MyCellCutscene.State.EndSpeakMayor) 
                _mayor.SetActive(true);
        }
    }
}