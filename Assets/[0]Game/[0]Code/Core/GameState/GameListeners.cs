namespace Game
{
    public interface IGameListener { }
        
    public interface IGameStartListener : IGameListener
    {
        void OnStartGame();
    }

    public interface IGamePauseListener : IGameListener
    {
        void OnPauseGame();
    }
    public interface IGameResumeListener : IGameListener
    {
        void OnResumeGame();
    }

    public interface IGameUpdateListener : IGameListener
    {
        void OnUpdate();
    }
    
    public interface IGameFixedUpdateListener : IGameListener
    {
        void OnFixedUpdate();
    }
    
    public interface IGameTransitionListener : IGameListener
    {
        void OnStartTransition();
        void OnEndTransition();
    }
    
    public interface IGameLaptopListener : IGameListener
    {
        void OnOpenLaptop();
        void OnCloseLaptop();
    }
    
    public interface IGameShopListener : IGameListener
    {
        void OnOpenShop();
        void OnCloseShop();
    }
    
    public interface IGameADSListener : IGameListener
    {
        void OnShowADS();
        void OnHideADS();
    }
    
    public interface IGameDialogueListener : IGameListener
    {
        void OnShowDialogue();
        void OnHideDialogue();
    }
}