using System;
using System.Collections.Generic;
using PixelCrushers.DialogueSystem;

namespace Game
{
    [Serializable]
    public class HatManager
    {
        public HatData GetCurrentHat => _currentHat;
        public IEnumerable<HatData> GetAllHat => _hats;
        
        public event Action<HatData> OnHatUpgrade;

        private IAnalyticsService _analyticsService;
        private HatData _currentHat;
        private List<HatData> _hats = new();
        private ISaveLoadService _saveLoadService;
        private List<string> _processLater = new();
        private bool _isInit;

        [Serializable]
        public struct Data
        {
            public bool IsBuy;
        }
        
        [Serializable]
        public struct CurrentHatData
        {
            public string Id;
        }
        
        [Inject]
        private void Construct(IAnalyticsService analyticsService, ISaveLoadService saveLoadService)
        {
            _analyticsService = analyticsService;
            _saveLoadService = saveLoadService;
        }
        
        public void Init()
        {
            _isInit = true;
            _currentHat = null;
            _hats.Add(null);

            foreach (var config in AssetProvider.Instance.HatConfigs)
            {
                config.Init();
                var data = RepositoryStorage.Get<Data>(config.GetId);
                _hats.Add(new HatData(config, data.IsBuy));
            }

            var currentId = RepositoryStorage.Get<CurrentHatData>(KeyConstants.CurrentHat).Id;
            foreach (var hat in _hats)
            {
                if (hat != null && hat.Config.GetId == currentId)
                {
                    UpgradeHat(hat);
                    break;
                }
            }

            foreach (var id in _processLater) 
                BuyHat(id);

            Lua.RegisterFunction(nameof(BuyHat), this,
                SymbolExtensions.GetMethodInfo(() => BuyHat(string.Empty)));
            
            Lua.RegisterFunction(nameof(IsBuyHat), this,
                SymbolExtensions.GetMethodInfo(() => IsBuyHat(string.Empty)));
        }

        public void UpgradeHat(HatData data)
        {
            _currentHat = data;

            RepositoryStorage.Set(KeyConstants.CurrentHat,
                _currentHat != null
                    ? new CurrentHatData() { Id = data.Config.GetId }
                    : new CurrentHatData() { Id = string.Empty });

            _saveLoadService.Save();
            OnHatUpgrade?.Invoke(data);
        }

        public void BuyHat(string id)
        {
            for (int i = 0; i < _hats.Count; i++)
            {
                if (_hats[i] != null && _hats[i].Config.GetId == id)
                {
                    _hats[i].IsBuy = true;
                    RepositoryStorage.Set(id, new Data() { IsBuy = true});
                    UpgradeHat(_hats[i]);
                    _analyticsService.Send("Buy", id);
                    return;
                }
            }

            if (!_isInit) 
                _processLater.Add(id);
        }

        public bool IsBuyHat(string id)
        {
            for (int i = 0; i < _hats.Count; i++)
            {
                if (_hats[i] != null && _hats[i].Config.GetId == id && _hats[i].IsBuy)
                    return true;
            }

            return false;
        }
    }
    
    public class HatData
    {
        public HatData(HatBaseConfig config, bool isBuy)
        {
            Config = config;
            IsBuy = isBuy;
        }
            
        public HatBaseConfig Config;
        public bool IsBuy;
    }
}