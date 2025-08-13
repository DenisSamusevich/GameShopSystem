using GameShopSystem.Domain.DomainEvemts;
using GameShopSystem.Domain.Entities;
using GameShopSystem.Domain.Services.Interfaces;

namespace GameShopSystem.Domain.Services
{
    public class RealMoneyPurchaseService : IRealMoneyPurchaseService
    {
        public void PurchaseItem(Player player, ShopItem shopItem, int finalPrice, string transactionId)
        {
            player.AddRealMoneyPurchasedEvent(new RealMoneyPurchaseEvent(
                player.Id,
                shopItem.Id,
                transactionId,
                finalPrice));
        }
    }
}
