namespace Game
{
    public sealed class LuckyBlock : BaseLuckyBlock
    {
        public override bool CanMining => !_stockMarketService.IsOpenItem("LuckyBlock");
        
        public override void AddPrize()
        {
            _stockMarketService.AddItem("LuckyBlock");
        }
    }
}