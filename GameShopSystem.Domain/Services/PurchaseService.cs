using GameShopSystem.Domain.DomainEvemts;
using GameShopSystem.Domain.Entities;
using GameShopSystem.Domain.Services.Interfaces;
using GameShopSystem.Domain.ValueObjects;

namespace GameShopSystem.Domain.Services
{
    public class PurchaseService : IPurchaseService
    {
        public void PurchaseItem(Player player, ShopItem shopItem, int finalPrice)
        {
            if (shopItem.CurrencyType == CurrencyType.RealMoney) throw new Exception("Invalid CurrencyType");
            switch (shopItem.CurrencyType)
            {
                case CurrencyType.Gold:
                    if (player.BalanceGold < shopItem.Price) throw new Exception("Not enough gold");
                    player.BalanceGold -= finalPrice;
                    break;
                case CurrencyType.Gems:
                    if (player.BalanceGems < shopItem.Price) throw new Exception("Not enough gems");
                    player.BalanceGems -= finalPrice;
                    break;
            }
            player.AddPurchasedEvent(new PurchaseEvent(
                player.Id,
                shopItem.Id,
                shopItem.CurrencyType,
                finalPrice));
        }
    }
}
