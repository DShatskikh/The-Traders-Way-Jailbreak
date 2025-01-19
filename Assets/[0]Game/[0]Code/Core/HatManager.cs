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

        [Inject]
        private void Construct(IAnalyticsService analyticsService)
        {
            _analyticsService = analyticsService;
        }
        
        public void Init()
        {
            _currentHat = null;
            _hats.Add(null);

            foreach (var config in AssetProvider.Instance.HatConfigs)
            {
                config.Init();
                _hats.Add(new HatData(config, false));
            }
            
            Lua.RegisterFunction(nameof(BuyHat), this,
                SymbolExtensions.GetMethodInfo(() => BuyHat(string.Empty)));
            
            Lua.RegisterFunction(nameof(IsBuyHat), this,
                SymbolExtensions.GetMethodInfo(() => IsBuyHat(string.Empty)));
        }

        public void UpgradeHat(HatData data)
        {
            _currentHat = data;
            OnHatUpgrade?.Invoke(data);
        }

        public void BuyHat(string id)
        {
            for (int i = 0; i < _hats.Count; i++)
            {
                if (_hats[i] != null && _hats[i].Config.GetId == id)
                {
                    _hats[i].IsBuy = true;
                    UpgradeHat(_hats[i]);
                    _analyticsService.Send("Buy", id);
                    return;
                }
            }
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