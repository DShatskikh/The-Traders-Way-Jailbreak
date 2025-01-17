using PixelCrushers.DialogueSystem;
using UnityEngine;

namespace Game
{
    public class LuaCommandRegister
    {
        private EndingsGame _endingsGame;

        [Inject]
        private void Construct(EndingsGame endingsGame)
        {
            _endingsGame = endingsGame;
        }

        public void Init()
        {
            Lua.RegisterFunction(nameof(OpenURL), this,
                SymbolExtensions.GetMethodInfo(() => OpenURL(string.Empty)));
            
            Lua.RegisterFunction(nameof(IsBuy), this,
                SymbolExtensions.GetMethodInfo(() => IsBuy(string.Empty)));
            
            Lua.RegisterFunction(nameof(IsFirstShopOpen), this,
                SymbolExtensions.GetMethodInfo(() => IsFirstShopOpen()));
            
            Lua.RegisterFunction(nameof(IsSkibidi), this,
                SymbolExtensions.GetMethodInfo(() => IsSkibidi()));
            
            Lua.RegisterFunction(nameof(IsBreakVase), this,
                SymbolExtensions.GetMethodInfo(() => IsBreakVase()));
            
            Lua.RegisterFunction(nameof(IsWorldPolice), this,
                SymbolExtensions.GetMethodInfo(() => IsWorldPolice()));
            
            Lua.RegisterFunction(nameof(IsSpeakDeveloper), this,
                SymbolExtensions.GetMethodInfo(() => IsSpeakDeveloper()));
            
            Lua.RegisterFunction(nameof(EndingGameStandard), this,
                SymbolExtensions.GetMethodInfo(() => EndingGameStandard()));
            
            Lua.RegisterFunction(nameof(EndingGameSecret), this,
                SymbolExtensions.GetMethodInfo(() => EndingGameSecret()));
            
            Lua.RegisterFunction(nameof(IsCompleteStandardEnding), this,
                SymbolExtensions.GetMethodInfo(() => IsCompleteStandardEnding()));
        }

        private void OpenURL(string address) => 
            Application.OpenURL(address);

        private bool IsBuy(string id) => 
            RepositoryStorage.Get<BuyPlate.SaveData>(id).IsBuy;

        private bool IsFirstShopOpen() => 
            !RepositoryStorage.Get<NoobikSkinShop.SaveData>("SkinShop").IsNotFirstOpen;

        private bool IsSkibidi() => 
            RepositoryStorage.Get<Skibidi.SaveData>("Skibidi").IsShow;

        private bool IsBreakVase() => 
            RepositoryStorage.Get<PrehistoricVase.SaveData>("Vase").IsBreak;

        private bool IsWorldPolice() => 
            RepositoryStorage.Get<HomeCutscene.SaveData>(KeyConstants.HomeCutscene)
                .CutsceneState == HomeCutscene.CutsceneState.POLICE;

        private bool IsSpeakDeveloper() => 
            RepositoryStorage.Get<HomeCutscene.SaveData>(KeyConstants.HomeCutscene)
                .CutsceneState == HomeCutscene.CutsceneState.PARTY;

        private void EndingGameStandard() => 
            _endingsGame.EndingGameStandard();

        private void EndingGameSecret() => 
            _endingsGame.EndingGameSecret();

        private bool IsCompleteStandardEnding() => 
            false;
    }
}