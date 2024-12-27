using UnityEngine;

namespace Game
{
    public class CoroutineRunner : MonoBehaviour
    {
        public static CoroutineRunner Instance { get; private set; }

        public void Init()
        {
            Instance = this;
        }
    }
}