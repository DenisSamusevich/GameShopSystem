using GameShopSystem.Domain.ValueObjects;

namespace GameShopSystem.Domain.Entities
{
    public class ShopItem
    {
        public Guid Id { get; }
        public int Price { get; set; }
        public CurrencyType CurrencyType { get; }
        public ShopItem(Guid id, int price, CurrencyType correncyType)
        {
            Id = id;
            if (Enum.IsDefined(typeof(CurrencyType), correncyType) == false) throw new Exception();
            CurrencyType = correncyType;
            if (price < 0) throw new Exception();
            Price = price;
        }
    }
}
