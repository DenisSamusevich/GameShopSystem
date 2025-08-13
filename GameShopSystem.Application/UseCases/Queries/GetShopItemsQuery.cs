using GameShopSystem.Application.DTOs;
using Mediator;

namespace GameShopSystem.Application.UseCases.Queries
{
    public record GetShopItemsQuery : IQuery<List<ShopItemDto>>
    {

    }
}
