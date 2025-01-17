using UnityEngine;

namespace Game
{
    public class AllBuyTestTest : MonoBehaviour
    {
        [ContextMenu("BuyAll")]
        private void BuyAll()
        {
            ServiceLocator.Get<AllBuyCheckHandler>().AllBuyTest();
        }
    }
}