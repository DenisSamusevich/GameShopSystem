using GameShopSystem.Domain.Entities;
using GameShopSystem.Domain.Services;
using GameShopSystem.Domain.ValueObjects;

namespace GameShopSystem.Test.Unit
{
    internal class PurchaseServiceTest
    {
        private PurchaseService _purchaseService;

        [SetUp]
        public void SetUp()
        {
            _purchaseService = new PurchaseService();
        }

        [Test]
        public void PurchaseItem_ValidItem_AddPurchaseEventToPlayer()
        {
            var balanceGold = 1000;
            var balanceGems = 1000;
            var finalPrice = 500;
            var player = new Player(Guid.NewGuid(), balanceGold, balanceGems);
            var shopItem = new ShopItem(Guid.NewGuid(), 500, CurrencyType.Gold);

            _purchaseService.PurchaseItem(player, shopItem, finalPrice);

            Assert.That(player.PurchaseEvents.Count, Is.EqualTo(1));
            Assert.That(player.PurchaseEvents[0].PlayerId, Is.EqualTo(player.Id));
            Assert.That(player.PurchaseEvents[0].ShopItemId, Is.EqualTo(shopItem.Id));
            Assert.That(player.PurchaseEvents[0].Price, Is.EqualTo(finalPrice));
            Assert.That(player.PurchaseEvents[0].CurrencyType, Is.EqualTo(CurrencyType.Gold));
            Assert.That(player.BalanceGold, Is.EqualTo(balanceGold - finalPrice));
            Assert.That(player.BalanceGems, Is.EqualTo(balanceGems));
        }

        [Test]
        public void PurchaseItem_NotEnoughBalance_ThrowsException()
        {
            var balanceGold = 1000;
            var balanceGems = 250;
            var finalPrice = 500;
            var player = new Player(Guid.NewGuid(), balanceGold, balanceGems);
            var shopItem = new ShopItem(Guid.NewGuid(), 500, CurrencyType.Gems);

            var exeption = Assert.Throws<Exception>(() =>
            {
                _purchaseService.PurchaseItem(player, shopItem, finalPrice);
            });

            Assert.That(exeption.Message, Is.EqualTo("Not enough gems"));
        }

        [Test]
        public void PurchaseItem_InvalidCurrencyType_ThrowsException()
        {
            var balanceGold = 1000;
            var balanceGems = 1000;
            var finalPrice = 500;
            var player = new Player(Guid.NewGuid(), balanceGold, balanceGems);
            var shopItem = new ShopItem(Guid.NewGuid(), 500, CurrencyType.RealMoney);

            var exeption = Assert.Throws<Exception>(() =>
            {
                _purchaseService.PurchaseItem(player, shopItem, finalPrice);
            });

            Assert.That(exeption.Message, Is.EqualTo("Invalid CurrencyType"));
        }
    }
}
