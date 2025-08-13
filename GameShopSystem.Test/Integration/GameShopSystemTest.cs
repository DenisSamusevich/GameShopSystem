using GameShopSystem.Application.DTOs;
using GameShopSystem.Application.Extensions;
using GameShopSystem.Application.Interfaces;
using GameShopSystem.Application.UseCases.Comands;
using GameShopSystem.Domain;
using GameShopSystem.Domain.Entities;
using GameShopSystem.Domain.RepositoriesInterfaces;
using GameShopSystem.Domain.ValueObjects;
using Mediator;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace GameShopSystem.Test.Integration
{
    internal class GameShopSystemTest
    {
        private ServiceProvider _serviceProvider;
        private Mock<IPlayerRepository> _playerRepositiryMock;
        private Mock<IShopItemRepository> _shopItemRepositoryMock;
        private Mock<IDiscountRepository> _discountRepositoryMock;
        private Mock<IPaymentGateway> _paymentGatewayMock;

        [SetUp]
        public void Setup()
        {
            _playerRepositiryMock = new Mock<IPlayerRepository>();
            _shopItemRepositoryMock = new Mock<IShopItemRepository>();
            _discountRepositoryMock = new Mock<IDiscountRepository>();
            _paymentGatewayMock = new Mock<IPaymentGateway>();

            var serviceCollection = new ServiceCollection();
            serviceCollection.AddDomainService()
                .AddApplicationServices()
                .AddSingleton(_playerRepositiryMock.Object)
                .AddSingleton(_shopItemRepositoryMock.Object)
                .AddSingleton(_discountRepositoryMock.Object)
                .AddSingleton(_paymentGatewayMock.Object);

            _serviceProvider = serviceCollection.BuildServiceProvider();
        }

        [TearDown]
        public void TearDown()
        {
            _serviceProvider?.Dispose();
        }

        [Test]
        public async Task PlayerPurchaseWithRealMoney_PlayerStatusChanged()
        {
            var balanceGold = 1000;
            var balanceGems = 1000;
            var player = new Player(Guid.NewGuid(), balanceGold, balanceGems);
            _playerRepositiryMock
                .Setup(p => p.GetPlayerByIdAsync(It.IsAny<Guid>()))
                .Returns(Task.FromResult<Player?>(player));
            _playerRepositiryMock.Setup(p => p.UpdateAsync(It.IsAny<Player>()))
                .Returns(Task.FromResult(true));
            var shopItem = new ShopItem(Guid.NewGuid(), 500, CurrencyType.Gold);
            _shopItemRepositoryMock
                .Setup(s => s.GetShopItemByIdAsync(It.IsAny<Guid>()))
                .Returns(Task.FromResult<ShopItem?>(shopItem));
            _discountRepositoryMock
                .Setup(d => d.GetAllDiscountsAsync())
                .Returns(Task.FromResult<IReadOnlyList<Discount>>(new List<Discount>()));

            var transactionId = "123456";
            _paymentGatewayMock.Setup(p => p.ChargeAsync(It.IsAny<Guid>(), It.IsAny<int>()))
                .Returns(Task.FromResult(new PaymentResultDto(true, transactionId)));

            var request = new PurchaseWithRealMoneyComand(player.Id, shopItem.Id);
            var mediator = _serviceProvider.GetRequiredService<IMediator>();

            var result = await mediator.Send(request);

            Assert.IsTrue(result);
            Assert.That(player.RealMoneyPurchaseEvents.Count, Is.EqualTo(1));
            Assert.That(player.RealMoneyPurchaseEvents[0].PlayerId, Is.EqualTo(player.Id));
            Assert.That(player.RealMoneyPurchaseEvents[0].ShopItemId, Is.EqualTo(shopItem.Id));
            Assert.That(player.RealMoneyPurchaseEvents[0].Price, Is.EqualTo(shopItem.Price));
            Assert.That(player.BalanceGold, Is.EqualTo(balanceGold));
            Assert.That(player.BalanceGems, Is.EqualTo(balanceGems));
            _playerRepositiryMock.Verify(p => p.GetPlayerByIdAsync(It.Is<Guid>(g => g.Equals(player.Id))), Times.Once);
            _shopItemRepositoryMock.Verify(s => s.GetShopItemByIdAsync(It.Is<Guid>(g => g.Equals(shopItem.Id))), Times.Once);
            _discountRepositoryMock.Verify(d => d.GetAllDiscountsAsync(), Times.Once);
            _paymentGatewayMock.Verify(p => p.ChargeAsync(It.Is<Guid>(g => g.Equals(player.Id)), It.Is<int>(p => p.Equals(shopItem.Price))), Times.Once);
            _playerRepositiryMock.Verify(p => p.UpdateAsync(It.Is<Player>(p => p.Id.Equals(player.Id))), Times.Once);
        }

        [Test]
        public async Task PlayerPurchaseWithGems_PlayerStatusChanged()
        {
            var balanceGold = 1000;
            var balanceGems = 1000;
            var player = new Player(Guid.NewGuid(), balanceGold, balanceGems);
            _playerRepositiryMock
                .Setup(p => p.GetPlayerByIdAsync(It.IsAny<Guid>()))
                .Returns(Task.FromResult<Player?>(player));
            _playerRepositiryMock.Setup(p => p.UpdateAsync(It.IsAny<Player>()))
                .Returns(Task.FromResult(true));
            var shopItem = new ShopItem(Guid.NewGuid(), 500, CurrencyType.Gems);
            _shopItemRepositoryMock
                .Setup(s => s.GetShopItemByIdAsync(It.IsAny<Guid>()))
                .Returns(Task.FromResult<ShopItem?>(shopItem));
            var discountPercentage = 25;
            var discountPrice = (int)(shopItem.Price * (1 - discountPercentage * 0.01f));
            _discountRepositoryMock
                .Setup(d => d.GetAllDiscountsAsync())
                .Returns(Task.FromResult<IReadOnlyList<Discount>>(new List<Discount>()
                {
                    new Discount(Guid.NewGuid(), new HashSet<Guid>(){shopItem.Id },CurrencyType.Gems,discountPercentage,DateTime.Now.AddDays(1))
                }));


            var request = new PurchaseItemComand(player.Id, shopItem.Id);
            var mediator = _serviceProvider.GetRequiredService<IMediator>();

            var result = await mediator.Send(request);

            Assert.IsTrue(result);
            Assert.That(player.PurchaseEvents.Count, Is.EqualTo(1));
            Assert.That(player.PurchaseEvents[0].PlayerId, Is.EqualTo(player.Id));
            Assert.That(player.PurchaseEvents[0].ShopItemId, Is.EqualTo(shopItem.Id));
            Assert.That(player.PurchaseEvents[0].Price, Is.EqualTo(discountPrice));
            Assert.That(player.PurchaseEvents[0].CurrencyType, Is.EqualTo(CurrencyType.Gems));
            Assert.That(player.BalanceGold, Is.EqualTo(balanceGold));
            Assert.That(player.BalanceGems, Is.EqualTo(balanceGems - discountPrice));
            _playerRepositiryMock.Verify(p => p.GetPlayerByIdAsync(It.Is<Guid>(g => g.Equals(player.Id))), Times.Once);
            _shopItemRepositoryMock.Verify(s => s.GetShopItemByIdAsync(It.Is<Guid>(g => g.Equals(shopItem.Id))), Times.Once);
            _discountRepositoryMock.Verify(d => d.GetAllDiscountsAsync(), Times.Once);
            _playerRepositiryMock.Verify(p => p.UpdateAsync(It.Is<Player>(p => p.Id.Equals(player.Id))), Times.Once);
        }
    }
}
