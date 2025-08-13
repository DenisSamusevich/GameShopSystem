using GameShopSystem.Application.Hundlers;
using GameShopSystem.Application.UseCases.Queries;
using GameShopSystem.Domain.Entities;
using GameShopSystem.Domain.RepositoriesInterfaces;
using GameShopSystem.Domain.Services.Interfaces;
using GameShopSystem.Domain.ValueObjects;
using Moq;

namespace GameShopSystem.Test.Unit
{
    internal class GetShopItemsHundlerTest
    {
        private Mock<IShopItemRepository> _shopItemRepository;
        private Mock<IDiscountRepository> _discountRepository;
        private Mock<IDiscountService> _discountService;

        [SetUp]
        public void Setup()
        {
            _shopItemRepository = new Mock<IShopItemRepository>();
            _discountService = new Mock<IDiscountService>();
            _discountRepository = new Mock<IDiscountRepository>();
        }

        [Test]
        public async Task Handle_ValidShopItems_AllShopItems()
        {
            var shopItemId_1 = Guid.NewGuid();
            var shopItemId_2 = Guid.NewGuid();
            var shopItemId_3 = Guid.NewGuid();
            var shopItemPrice_1 = 1001;
            var shopItemPrice_2 = 1002;
            var shopItemPrice_3 = 1003;
            var price = 500;
            _shopItemRepository
                .Setup(s => s.GetAllShopItemAsync())
                .Returns(Task.FromResult<IReadOnlyList<ShopItem>>(new List<ShopItem>()
                {
                    new ShopItem(shopItemId_1, shopItemPrice_1, CurrencyType.Gold),
                    new ShopItem(shopItemId_2, shopItemPrice_2, CurrencyType.Gems),
                    new ShopItem(shopItemId_3, shopItemPrice_3, CurrencyType.Gems),
                }));
            _discountRepository
                .Setup(d => d.GetAllDiscountsAsync())
                .Returns(Task.FromResult<IReadOnlyList<Discount>>(new List<Discount>()));
            _discountService.Setup(p => p.GetActualPrice(It.IsAny<ShopItem>(), It.IsAny<IEnumerable<Discount>>()))
                .Returns(price);

            var purchaseItemHundler = new GetShopItemsHandler(
                _shopItemRepository.Object,
                _discountRepository.Object,
                _discountService.Object);
            var request = new GetShopItemsQuery();

            var result = await purchaseItemHundler.Handle(request, default);

            Assert.That(result.Count == 3);
            Assert.That(result[0].ItemGuid, Is.EqualTo(shopItemId_1));
            Assert.That(result[0].Price, Is.EqualTo(shopItemPrice_1));
            Assert.That(result[0].DiscountPrice, Is.EqualTo(price));
            Assert.That(result[1].ItemGuid, Is.EqualTo(shopItemId_2));
            Assert.That(result[1].Price, Is.EqualTo(shopItemPrice_2));
            Assert.That(result[1].DiscountPrice, Is.EqualTo(price));
            Assert.That(result[2].ItemGuid, Is.EqualTo(shopItemId_3));
            Assert.That(result[2].Price, Is.EqualTo(shopItemPrice_3));
            Assert.That(result[2].DiscountPrice, Is.EqualTo(price));
            _shopItemRepository.Verify(s => s.GetAllShopItemAsync(), Times.Once);
            _discountRepository.Verify(d => d.GetAllDiscountsAsync(), Times.Once);
            _discountService.Verify(d => d.GetActualPrice(It.Is<ShopItem>(s => s.Id.Equals(shopItemId_2) || s.Id.Equals(shopItemId_3)), It.IsAny<IEnumerable<Discount>>()), Times.Exactly(2));
        }
    }
}
