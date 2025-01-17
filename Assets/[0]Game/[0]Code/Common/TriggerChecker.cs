using System;
using UnityEngine;

namespace Game
{
    public sealed class TriggerChecker : MonoBehaviour
    {
        public Action<GameObject> TriggerEnter;

        private void OnTriggerEnter2D(Collider2D other) => 
            TriggerEnter?.Invoke(other.gameObject);
    }
}