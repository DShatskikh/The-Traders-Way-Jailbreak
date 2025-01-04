using UnityEngine;

namespace Game
{
    public class UseTest : MonoBehaviour, IUseObject
    {
        public void Use()
        {
            print("Test Use");
        }
    }
}