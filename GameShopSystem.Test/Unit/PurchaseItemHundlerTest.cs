using GameShopSystem.Application.Hundlers;
using GameShopSystem.Application.UseCases.Comands;
using GameShopSystem.Domain.Entities;
using GameShopSystem.Domain.RepositoriesInterfaces;
using GameShopSystem.Domain.Services.Interfaces;
using GameShopSystem.Domain.ValueObjects;
using Moq;

namespace GameShopSystem.Test.Unit
{
    internal class PurchaseItemHundlerTest
    {
        private Mock<IPlayerRepository> _playerRepositiryMock;
        private Mock<IShopItemRepository> _shopItemRepository;
        private Mock<IDiscountRepository> _discountRepository;
        private Mock<IDiscountService> _discountService;
        private Mock<IPurchaseService> _purchaseService;

        [SetUp]
        public void Setup()
        {
            _playerRepositiryMock = new Mock<IPlayerRepository>();
            _shopItemRepository = new Mock<IShopItemRepository>();
            _discountRepository = new Mock<IDiscountRepository>();
            _discountService = new Mock<IDiscountService>();
            _purchaseService = new Mock<IPurchaseService>();
        }

        [Test]
        public async Task Handle_ValidShopItemAndPlayer_CallUpdatePlayer()
        {
            var playerId = Guid.NewGuid();
            _playerRepositiryMock
                .Setup(p => p.GetPlayerByIdAsync(It.IsAny<Guid>()))
                .Returns(Task.FromResult<Player?>(new Player(playerId, 1000, 1000)));
            _playerRepositiryMock.Setup(p => p.UpdateAsync(It.IsAny<Player>()))
                .Returns(Task.FromResult(true));

            var shopItemId = Guid.NewGuid();
            var price = 500;
            _shopItemRepository
                .Setup(s => s.GetShopItemByIdAsync(It.IsAny<Guid>()))
                .Returns(Task.FromResult<ShopItem?>(new ShopItem(shopItemId, price, CurrencyType.Gold)));
            _discountRepository
                .Setup(d => d.GetAllDiscountsAsync())
                .Returns(Task.FromResult<IReadOnlyList<Discount>>(new List<Discount>()));

            _discountService.Setup(p => p.GetActualPrice(It.IsAny<ShopItem>(), It.IsAny<IEnumerable<Discount>>()))
                .Returns(price);

            _purchaseService.Setup(p => p.PurchaseItem(It.IsAny<Player>(), It.IsAny<ShopItem>(), It.IsAny<int>()));

            var purchaseItemHundler = new PurchaseItemHandler(
                _playerRepositiryMock.Object,
                _shopItemRepository.Object,
                _discountRepository.Object,
                _discountService.Object,
                _purchaseService.Object);
            var request = new PurchaseItemComand(playerId, shopItemId);

            var result = await purchaseItemHundler.Handle(request, default);

            Assert.IsTrue(result);
            _playerRepositiryMock.Verify(p => p.GetPlayerByIdAsync(It.Is<Guid>(g => g.Equals(playerId))), Times.Once);
            _shopItemRepository.Verify(s => s.GetShopItemByIdAsync(It.Is<Guid>(g => g.Equals(shopItemId))), Times.Once);
            _discountRepository.Verify(d => d.GetAllDiscountsAsync(), Times.Once);
            _discountService.Verify(d => d.GetActualPrice(It.Is<ShopItem>(s => s.Id.Equals(shopItemId)), It.IsAny<IEnumerable<Discount>>()), Times.Once);
            _purchaseService.Verify(p => p.PurchaseItem(It.Is<Player>(p => p.Id.Equals(playerId)),
                It.Is<ShopItem>(s => s.Id.Equals(shopItemId)), It.Is<int>(p => p.Equals(price))), Times.Once);
            _playerRepositiryMock.Verify(p => p.UpdateAsync(It.Is<Player>(p => p.Id.Equals(playerId))), Times.Once);
        }
    }
}
