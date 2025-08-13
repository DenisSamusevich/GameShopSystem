using GameShopSystem.Domain.Entities;
using GameShopSystem.Domain.Services;
using GameShopSystem.Domain.ValueObjects;

namespace GameShopSystem.Test.Unit
{
    internal class DiscountServiceTest
    {
        private DiscountService _discountService;
        private List<Discount> _discounts;

        [SetUp]
        public void SetUp()
        {
            _discounts = new List<Discount>();
            _discountService = new DiscountService();
        }

        [Test]
        public void GetActualPrice_NoDiscount_PriceInchanged()
        {
            var shopItemPrice = 1000;
            var shopItem = new ShopItem(Guid.NewGuid(), shopItemPrice, CurrencyType.Gold);

            var price = _discountService.GetActualPrice(shopItem, _discounts);

            Assert.That(price, Is.EqualTo(shopItemPrice));
        }
        [Test]
        public async Task GetActualPrice_ExpiredDiscount_PriceInchanged()
        {
            var shopItemPrice = 1000;
            var shopItem = new ShopItem(Guid.NewGuid(), shopItemPrice, CurrencyType.Gems);
            _discounts.Add(new Discount(Guid.NewGuid(),
                new HashSet<Guid>() { shopItem.Id, },
                CurrencyType.Gems,
                50,
                DateTime.Now.AddMicroseconds(500)));
            await Task.Delay(500);
            var price = _discountService.GetActualPrice(shopItem, _discounts);

            Assert.That(price, Is.EqualTo(shopItemPrice));
        }
        [Test]
        public void GetActualPrice_50PercentDiscount_PriceChanged()
        {
            var shopItemPrice = 1000;
            var discount_50 = 50;
            var shopItem = new ShopItem(Guid.NewGuid(), shopItemPrice, CurrencyType.RealMoney);
            _discounts.Add(new Discount(Guid.NewGuid(),
                new HashSet<Guid>() { shopItem.Id, },
                CurrencyType.RealMoney,
                discount_50,
                DateTime.Now.AddDays(1)));
            int totalPrice = (int)(shopItem.Price * (1 - 0.01f * discount_50));

            var price = _discountService.GetActualPrice(shopItem, _discounts);

            Assert.That(price, Is.EqualTo(totalPrice));
        }
        [Test]
        public void GetActualPrice_MultipleDiscounts_LargestDiscountIsApplied()
        {
            var shopItemPrice = 1000;
            var discount_50 = 50;

            var shopItem = new ShopItem(Guid.NewGuid(), shopItemPrice, CurrencyType.Gold);
            _discounts.Add(new Discount(Guid.NewGuid(),
                new HashSet<Guid>() { shopItem.Id, },
                CurrencyType.Gold,
                25,
                DateTime.Now.AddDays(1)));
            _discounts.Add(new Discount(Guid.NewGuid(),
                new HashSet<Guid>() { shopItem.Id, },
                CurrencyType.Gold,
                discount_50,
                DateTime.Now.AddDays(2)));
            int totalPrice = (int)(shopItem.Price * (1 - 0.01f * discount_50));

            var price = _discountService.GetActualPrice(shopItem, _discounts);

            Assert.That(price, Is.EqualTo(totalPrice));
        }
    }
}
