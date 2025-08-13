using GameShopSystem.Domain.Entities;

namespace GameShopSystem.Domain.Services.Interfaces
{
    public interface IPurchaseService
    {
        public void PurchaseItem(Player player, ShopItem shopItem, int finalPrice);
    }
}
