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
        }
        
        private void OpenURL(string address) => 
            Application.OpenURL(address);
    }
}