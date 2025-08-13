using GameShopSystem.Application.Interfaces;
using GameShopSystem.Application.UseCases.Comands;
using GameShopSystem.Domain.RepositoriesInterfaces;
using GameShopSystem.Domain.Services.Interfaces;
using Mediator;

namespace GameShopSystem.Application.Hundlers
{
    public class PurchaseWithRealMoneyHandler : ICommandHandler<PurchaseWithRealMoneyComand, bool>
    {
        private readonly IPlayerRepository _playerRepository;
        private readonly IShopItemRepository _shopItemRepository;
        private readonly IDiscountRepository _discountRepository;
        private readonly IPaymentGateway _paymentGateway;
        private readonly IDiscountService _discountService;
        private readonly IRealMoneyPurchaseService _realMoneyPurchaseService;

        public PurchaseWithRealMoneyHandler(
            IPlayerRepository playerRepository,
            IShopItemRepository shopItemRepository,
            IDiscountRepository discountRepository,
            IPaymentGateway paymentGateway,
            IDiscountService discountService,
            IRealMoneyPurchaseService realMoneyPurchaseService)
        {
            _playerRepository = playerRepository;
            _shopItemRepository = shopItemRepository;
            _discountRepository = discountRepository;
            _paymentGateway = paymentGateway;
            _discountService = discountService;
            _realMoneyPurchaseService = realMoneyPurchaseService;
        }

        public async ValueTask<bool> Handle(PurchaseWithRealMoneyComand command, CancellationToken cancellationToken)
        {
            var player = await _playerRepository.GetPlayerByIdAsync(command.PlayerId);
            if (player == null) return false;
            if (cancellationToken.IsCancellationRequested) return false;
            var shopItem = await _shopItemRepository.GetShopItemByIdAsync(command.ShopItemId);
            if (shopItem == null) return false;
            if (cancellationToken.IsCancellationRequested) return false;
            var discounts = await _discountRepository.GetAllDiscountsAsync();
            if (cancellationToken.IsCancellationRequested) return false;
            var finalPrice = _discountService.GetActualPrice(shopItem, discounts);
            if (cancellationToken.IsCancellationRequested) return false;
            var paymentResult = await _paymentGateway.ChargeAsync(player.Id, finalPrice);
            if (cancellationToken.IsCancellationRequested) return false;
            if (paymentResult.IsSuccess)
            {
                _realMoneyPurchaseService.PurchaseItem(player, shopItem, finalPrice, paymentResult.TransactionId);
                return await _playerRepository.UpdateAsync(player);
            }
            else
            {
                return false;
            }
        }
    }
}
