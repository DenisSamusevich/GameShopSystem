namespace GameShopSystem.Domain.DomainEvemts
{
    public record RealMoneyPurchaseEvent
    {
        public Guid PlayerId { get; }
        public Guid ShopItemId { get; }
        public string TransactionId { get; }
        public int Price { get; }

        public RealMoneyPurchaseEvent(
            Guid playerId,
            Guid shopItemId,
            string transactionId,
            int price)
        {
            PlayerId = playerId;
            ShopItemId = shopItemId;
            TransactionId = transactionId;
            Price = price;
        }
    }
}
