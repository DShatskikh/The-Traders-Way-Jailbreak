namespace Game
{
    public class SecretLuckyBlock : BaseLuckyBlock
    {
        public override bool CanMining => true;
        public override void AddPrize()
        {
            var hatManager = ServiceLocator.Get<HatManager>();
            hatManager.BuyHat("CoolGlasses");
        }
    }
}