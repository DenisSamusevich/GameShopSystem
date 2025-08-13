using GameShopSystem.Domain.Entities;
using GameShopSystem.Domain.Services;
using GameShopSystem.Domain.ValueObjects;

namespace GameShopSystem.Test.Unit
{
    internal class RealMoneyPurchaseServiceTest
    {
        private RealMoneyPurchaseService _realMoneyPurchaseService;

        [SetUp]
        public void Setup()
        {
            _realMoneyPurchaseService = new RealMoneyPurchaseService();
        }

        [Test]
        public void PurchaseItem_ValidItem_AddPurchaseEventToPlayer()
        {
            var balanceGold = 1000;
            var balanceGems = 1000;
            var finalPrice = 500;
            var transactionId = "123456";
            var player = new Player(Guid.NewGuid(), balanceGold, balanceGems);
            var shopItem = new ShopItem(Guid.NewGuid(), 500, CurrencyType.Gold);

            _realMoneyPurchaseService.PurchaseItem(player, shopItem, finalPrice, transactionId);

            Assert.That(player.RealMoneyPurchaseEvents.Count, Is.EqualTo(1));
            Assert.That(player.RealMoneyPurchaseEvents[0].PlayerId, Is.EqualTo(player.Id));
            Assert.That(player.RealMoneyPurchaseEvents[0].ShopItemId, Is.EqualTo(shopItem.Id));
            Assert.That(player.RealMoneyPurchaseEvents[0].Price, Is.EqualTo(finalPrice));
            Assert.That(player.RealMoneyPurchaseEvents[0].TransactionId, Is.EqualTo(transactionId));
        }
    }
}
