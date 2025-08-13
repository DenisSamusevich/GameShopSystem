using GameShopSystem.Application.UseCases.Comands;
using GameShopSystem.Domain.RepositoriesInterfaces;
using GameShopSystem.Domain.Services.Interfaces;
using Mediator;

namespace GameShopSystem.Application.Hundlers
{
    public class PurchaseItemHandler : ICommandHandler<PurchaseItemComand, bool>
    {
        private readonly IPlayerRepository _playerRepository;
        private readonly IShopItemRepository _shopItemRepository;
        private readonly IDiscountRepository _discountRepository;
        private readonly IDiscountService _discountService;
        private readonly IPurchaseService _purchaseService;

        public PurchaseItemHandler(
            IPlayerRepository playerRepository,
            IShopItemRepository shopItemRepository,
            IDiscountRepository discountRepository,
            IDiscountService discountService,
            IPurchaseService purchaseService)
        {
            _playerRepository = playerRepository;
            _shopItemRepository = shopItemRepository;
            _discountRepository = discountRepository;
            _discountService = discountService;
            _purchaseService = purchaseService;
        }

        public async ValueTask<bool> Handle(PurchaseItemComand command, CancellationToken cancellationToken)
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
            _purchaseService.PurchaseItem(player, shopItem, finalPrice);
            return await _playerRepository.UpdateAsync(player);

        }
    }
}
