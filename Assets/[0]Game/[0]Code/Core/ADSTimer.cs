using System;
using System.Collections;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class ADSTimer : IGameStartListener
    {
        [SerializeField]
        private float _duration = 1f;
        
        public void OnStartGame()
        {
            CoroutineRunner.Instance.StartCoroutine(AwaitTime());
        }

        private IEnumerator AwaitTime()
        {
            while (true)
            {
                yield return new WaitForSeconds(_duration);
                Debug.Log("Реклама");  
            }
        }
    }
}