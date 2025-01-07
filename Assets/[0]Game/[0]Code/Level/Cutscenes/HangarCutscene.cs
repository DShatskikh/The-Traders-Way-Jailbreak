using System;
using UnityEngine;

namespace Game
{
    public class HangarCutscene : MonoBehaviour
    {
        private SaveData _saveData;
        
        [Serializable]
        public struct SaveData
        {
            public bool IsEnd;
        }

        private void Start()
        {
            
        }
    }
}