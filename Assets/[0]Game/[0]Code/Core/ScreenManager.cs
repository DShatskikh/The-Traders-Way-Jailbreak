using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class ScreenManager
    {
        [SerializeField]
        private SerializablePair<ScreenType, ScreenBase>[] _windows;

        public void Show(ScreenType type, object args = null, bool hideOther = true)
        {
            if (hideOther)
                HideAll();

            foreach (var pair in _windows)
            {
                if (pair.Key == type)
                {
                    pair.Value.Show();
                    pair.Value.Bind(args);
                }
            }
        }

        public void Hide(ScreenType type)
        {
            foreach (var pair in _windows)
            {
                if (pair.Key == type) 
                    pair.Value.Hide();
            }
        }
        
        public void HideAll()
        {
            foreach (var pair in _windows)
                pair.Value.Hide();
        }
    }
}