using System;
using UnityEngine;

namespace Game
{
    public class TriggerChecker : MonoBehaviour
    {
        public Action<GameObject> TriggerEnter;

        private void OnTriggerEnter2D(Collider2D other) => 
            TriggerEnter?.Invoke(other.gameObject);
    }
}