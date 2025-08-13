using GameShopSystem.Domain.ValueObjects;

namespace GameShopSystem.Domain.DomainEvemts
{
    public record PurchaseEvent
    {
        public Guid PlayerId { get; }
        public Guid ShopItemId { get; }
        public CurrencyType CurrencyType { get; }
        public int Price { get; }
        public PurchaseEvent(
            Guid playerId,
            Guid shopItemId,
            CurrencyType currencyType,
            int price)
        {
            PlayerId = playerId;
            ShopItemId = shopItemId;
            CurrencyType = currencyType;
            Price = price;
        }
    }
}