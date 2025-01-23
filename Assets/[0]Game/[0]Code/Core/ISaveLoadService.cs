using System;
using System.Collections.Generic;
using UnityEngine;
using YG;

namespace Game
{
    public interface ISaveLoadService
    {
        void Load();
        void Save();
        void Reset();
    }

    [Serializable]
    public sealed class SaveLoadService : ISaveLoadService
    {
        [SerializeField]
        private AllInitData _initData;

        private GameStateController _gameStateController;
        private LocationLoader _locationLoader;

        [Inject]
        private void Construct(GameStateController gameStateController, LocationLoader locationLoader)
        {
            _gameStateController = gameStateController;
            _locationLoader = locationLoader;
        }
        
        public void Load()
        {
            Debug.Log("Load");

            if (RepositoryStorage.Get<FirstOpen>(KeyConstants.IsNotFirstOpen).IsNotFirstOpen)
            {
                _locationLoader.Load();
            }
            else
            {
                Reset();
                RepositoryStorage.Set(KeyConstants.Volume, _initData.Volume);
                RepositoryStorage.Set(KeyConstants.Ending, _initData.EndsData);
                RepositoryStorage.Set(KeyConstants.IsNotFirstOpen, new FirstOpen { IsNotFirstOpen = true });
                
                _gameStateController.OpenMainMenu();
            }
        }

        public void Save()
        {
            Debug.Log("Save");
            YG2.SaveProgress();
        }

        public void Reset()
        {
            Debug.Log("Reset");
            
            RepositoryStorage.Set(KeyConstants.HomeCutscene, _initData.HomeData);
            RepositoryStorage.Set(KeyConstants.MyCellCutscene, _initData.MyCellData);
            RepositoryStorage.Set(KeyConstants.SkinShop, _initData.NoobikData);
            RepositoryStorage.Set(KeyConstants.Name, _initData.PlayerName);
            RepositoryStorage.Set(KeyConstants.Location, _initData.LocationData);
            RepositoryStorage.Set(KeyConstants.Wallet, new WalletService.Data(_initData.StartMoney, _initData.StartTax, _initData.StartMoney));
            RepositoryStorage.Set(KeyConstants.IsNotFirstOpen, new FirstOpen() { IsNotFirstOpen = false });
            RepositoryStorage.Set(KeyConstants.StockMarket, new StockMarketService.Data());
            RepositoryStorage.Set(KeyConstants.Vase, new PrehistoricVase.SaveData() { IsBreak = false});

            foreach (var id in AssetProvider.Instance.PlatesId)
                RepositoryStorage.Set(id, new BuyPlate.SaveData() { IsBuy = false });
        }
    }

    [Serializable]
    public struct AllInitData
    {
        [Header("Reset")]
        public string PlayerName; //"Денис"
        public LocationsManager.Data LocationData;
        public HomeCutscene.SaveData HomeData;
        public MyCellCutscene.SaveData MyCellData;
        public NoobikSkinShop.SaveData NoobikData;
        public double StartMoney; //999999999
        public double StartTax; //999999999
        
        [Header("Not Reset")]
        public EndsData EndsData;
        public VolumeData Volume;
    }
}