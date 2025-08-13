using GameShopSystem.Application.DTOs;
using GameShopSystem.Application.UseCases.Queries;
using GameShopSystem.Domain.RepositoriesInterfaces;
using GameShopSystem.Domain.Services.Interfaces;
using Mediator;

namespace GameShopSystem.Application.Hundlers
{
    public class GetShopItemsHandler : IQueryHandler<GetShopItemsQuery, List<ShopItemDto>>
    {
        private readonly IShopItemRepository _shopItemRepository;
        private readonly IDiscountRepository _discountRepository;
        private readonly IDiscountService _discountService;

        public GetShopItemsHandler(IShopItemRepository shopItemRepository,
            IDiscountRepository discountRepository,
            IDiscountService discountService)
        {
            _shopItemRepository = shopItemRepository;
            _discountRepository = discountRepository;
            _discountService = discountService;
        }

        public async ValueTask<List<ShopItemDto>> Handle(GetShopItemsQuery query, CancellationToken cancellationToken)
        {
            var result = new List<ShopItemDto>();
            var shopItems = await _shopItemRepository.GetAllShopItemAsync();
            if (cancellationToken.IsCancellationRequested) return result;
            var discounts = await _discountRepository.GetAllDiscountsAsync();
            if (cancellationToken.IsCancellationRequested) return result;
            foreach (var shopItem in shopItems)
            {
                var finalPrice = _discountService.GetActualPrice(shopItem, discounts);
                result.Add(new ShopItemDto(shopItem.Id, shopItem.Price, finalPrice));
                if (cancellationToken.IsCancellationRequested) return result;
            }
            return result;
        }
    }
}
