using PixelCrushers.DialogueSystem;
using UnityEngine;

namespace Game
{
    public class LuaCommandRegister
    {
        public void Register()
        {
            Lua.RegisterFunction(nameof(OpenURL), this,
                SymbolExtensions.GetMethodInfo(() => OpenURL(string.Empty)));
            
            Lua.RegisterFunction(nameof(IsBuy), this,
                SymbolExtensions.GetMethodInfo(() => IsBuy(string.Empty)));
            
            Lua.RegisterFunction(nameof(IsFirstShopOpen), this,
                SymbolExtensions.GetMethodInfo(() => IsFirstShopOpen()));
            
            Lua.RegisterFunction(nameof(IsSkibidi), this,
                SymbolExtensions.GetMethodInfo(() => IsSkibidi()));
        }
        
        private void OpenURL(string address) => 
            Application.OpenURL(address);

        private bool IsBuy(string id) => 
            CutscenesDataStorage.GetData<BuyPlate.SaveData>(id).IsBuy;
        
        private bool IsFirstShopOpen() => 
            !CutscenesDataStorage.GetData<SkinShop.SaveData>("SkinShop").IsNotFirstOpen;
        
        private bool IsSkibidi() => 
            CutscenesDataStorage.GetData<Skibidi.SaveData>("Skibidi").IsShow;
    }
}