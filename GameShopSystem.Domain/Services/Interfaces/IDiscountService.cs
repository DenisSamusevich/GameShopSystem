using GameShopSystem.Domain.Entities;

namespace GameShopSystem.Domain.Services.Interfaces
{
    public interface IDiscountService
    {
        public int GetActualPrice(ShopItem item, IEnumerable<Discount> discounts);
    }
}
