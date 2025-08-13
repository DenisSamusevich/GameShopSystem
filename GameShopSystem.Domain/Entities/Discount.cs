using GameShopSystem.Domain.ValueObjects;

namespace GameShopSystem.Domain.Entities
{
    public record Discount
    {
        public Guid Id { get; }
        public IReadOnlySet<Guid> ShopItemIds { get; }
        public CurrencyType CurrencyType { get; }
        public float Percentage { get; }
        public DateTime ExpireDate { get; }

        public Discount(Guid id, HashSet<Guid> shopItemIds, CurrencyType correncyType, float percentage, DateTime expireDate)
        {
            Id = id;
            if (shopItemIds == null || shopItemIds.Count < 1) throw new Exception("shopItemIds is not correct");
            ShopItemIds = shopItemIds;
            if (Enum.IsDefined(typeof(CurrencyType), correncyType) == false) throw new Exception("correncyTarget is not correct");
            CurrencyType = correncyType;
            if (percentage <= 0 || percentage > 100) throw new Exception("percentage is not correct");
            Percentage = percentage;
            if (DateTime.Now > expireDate) throw new Exception("expireDate is not correct");
            ExpireDate = expireDate;
        }
    }
}
