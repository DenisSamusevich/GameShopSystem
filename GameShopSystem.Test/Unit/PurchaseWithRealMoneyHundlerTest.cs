using GameShopSystem.Application.DTOs;
using GameShopSystem.Application.Hundlers;
using GameShopSystem.Application.Interfaces;
using GameShopSystem.Application.UseCases.Comands;
using GameShopSystem.Domain.Entities;
using GameShopSystem.Domain.RepositoriesInterfaces;
using GameShopSystem.Domain.Services.Interfaces;
using GameShopSystem.Domain.ValueObjects;
using Moq;

namespace GameShopSystem.Test.Unit
{
    internal class PurchaseWithRealMoneyHundlerTest
    {
        private Mock<IPlayerRepository> _playerRepositiryMock;
        private Mock<IShopItemRepository> _shopItemRepository;
        private Mock<IDiscountRepository> _discountRepository;
        private Mock<IPaymentGateway> _paymentGateway;
        private Mock<IDiscountService> _discountService;
        private Mock<IRealMoneyPurchaseService> _realMoneyPurchaseService;

        [SetUp]
        public void Setup()
        {
            _playerRepositiryMock = new Mock<IPlayerRepository>();
            _shopItemRepository = new Mock<IShopItemRepository>();
            _discountRepository = new Mock<IDiscountRepository>();
            _paymentGateway = new Mock<IPaymentGateway>();
            _discountService = new Mock<IDiscountService>();
            _realMoneyPurchaseService = new Mock<IRealMoneyPurchaseService>();
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

            var transactionId = "123456";
            _paymentGateway.Setup(p => p.ChargeAsync(It.IsAny<Guid>(), It.IsAny<int>()))
                .Returns(Task.FromResult(new PaymentResultDto(true, transactionId)));

            _discountService.Setup(p => p.GetActualPrice(It.IsAny<ShopItem>(), It.IsAny<IEnumerable<Discount>>()))
                .Returns(price);

            _realMoneyPurchaseService.Setup(p => p.PurchaseItem(It.IsAny<Player>(), It.IsAny<ShopItem>(), It.IsAny<int>(), It.IsAny<string>()));

            var purchaseWithRealMoneyHandler = new PurchaseWithRealMoneyHandler(
                _playerRepositiryMock.Object,
                _shopItemRepository.Object,
                _discountRepository.Object,
                _paymentGateway.Object,
                _discountService.Object,
                _realMoneyPurchaseService.Object);
            var request = new PurchaseWithRealMoneyComand(playerId, shopItemId);

            var result = await purchaseWithRealMoneyHandler.Handle(request, default);

            Assert.IsTrue(result);
            _playerRepositiryMock.Verify(p => p.GetPlayerByIdAsync(It.Is<Guid>(g => g.Equals(playerId))), Times.Once);
            _shopItemRepository.Verify(s => s.GetShopItemByIdAsync(It.Is<Guid>(g => g.Equals(shopItemId))), Times.Once);
            _discountRepository.Verify(d => d.GetAllDiscountsAsync(), Times.Once);
            _paymentGateway.Verify(p => p.ChargeAsync(It.Is<Guid>(g => g.Equals(playerId)), It.Is<int>(p => p.Equals(price))), Times.Once);
            _discountService.Verify(d => d.GetActualPrice(It.Is<ShopItem>(s => s.Id.Equals(shopItemId)), It.IsAny<IEnumerable<Discount>>()), Times.Once);
            _realMoneyPurchaseService.Verify(p => p.PurchaseItem(It.Is<Player>(p => p.Id.Equals(playerId)),
                It.Is<ShopItem>(s => s.Id.Equals(shopItemId)), It.Is<int>(p => p.Equals(price)), It.Is<string>(t => t.Equals(transactionId))), Times.Once);
            _playerRepositiryMock.Verify(p => p.UpdateAsync(It.Is<Player>(p => p.Id.Equals(playerId))), Times.Once);
        }
    }
}
