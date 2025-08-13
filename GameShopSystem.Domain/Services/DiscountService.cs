using GameShopSystem.Domain.Entities;
using GameShopSystem.Domain.Services.Interfaces;

namespace GameShopSystem.Domain.Services
{
    public class DiscountService : IDiscountService
    {
        public int GetActualPrice(ShopItem item, IEnumerable<Discount> discounts)
        {
            var discount = discounts.Where(d => d.ShopItemIds.Contains(item.Id) && d.CurrencyType.HasFlag(item.CurrencyType) && d.ExpireDate > DateTime.Now)
                .OrderBy(d => d.Percentage).LastOrDefault();
            return discount != null ? (int)(item.Price * (1 - discount.Percentage * 0.01f)) : item.Price;
        }
    }
}
