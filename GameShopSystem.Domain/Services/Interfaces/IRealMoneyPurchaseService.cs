using GameShopSystem.Domain.Entities;

namespace GameShopSystem.Domain.Services.Interfaces
{
    public interface IRealMoneyPurchaseService
    {
        public void PurchaseItem(Player player, ShopItem shopItem, int finalPrice, string transactionId);
    }
}
