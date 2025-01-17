using System;
using PixelCrushers.DialogueSystem;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class AllBuyCheckHandler : IDisposable
    {
        [SerializeField]
        private DialogueSystemTrigger _allBuyDialogue;
        
        [SerializeField]
        private string[] _allId;

        public event Action AllBuy;

        [Inject]
        private void Construct()
        {
            BuyPlate.BuyEvent += OnBuy;
        }

        void IDisposable.Dispose()
        {
            BuyPlate.BuyEvent -= OnBuy;
        }

        public void OnBuy()
        {
            bool isAllBuy = true;
            int countBuy = 0;
            
            foreach (var id in _allId)
            {
                var data = RepositoryStorage.Get<BuyPlate.SaveData>(id);

                if (!data.IsBuy)
                {
                    isAllBuy = false;
                    continue;
                }
                else
                {
                    countBuy += 1;
                }
            }

            if (isAllBuy)
            {
                var data = RepositoryStorage.Get<MyCellCutscene.SaveData>(KeyConstants.MyCellCutscene);
                data.State = MyCellCutscene.State.DontPayTax;
                RepositoryStorage.Set(KeyConstants.MyCellCutscene, data);
                _allBuyDialogue.OnUse();
                AllBuy?.Invoke();
            }

            Debug.Log($"Куплено {countBuy} из {_allId.Length}");
        }
        
        public void AllBuyTest()
        {
            foreach (var id in _allId)
            {
                var data = RepositoryStorage.Get<BuyPlate.SaveData>(id);
                data.IsBuy = true;
                RepositoryStorage.Set(id, data);
            }

            BuyPlate.BuyEvent?.Invoke();
        }
    }
}