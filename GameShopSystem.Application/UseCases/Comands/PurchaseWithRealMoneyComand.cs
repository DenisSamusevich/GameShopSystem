using Mediator;

namespace GameShopSystem.Application.UseCases.Comands
{
    public record PurchaseWithRealMoneyComand : ICommand<bool>
    {
        public Guid PlayerId { get; }
        public Guid ShopItemId { get; }

        public PurchaseWithRealMoneyComand(Guid playerId, Guid shopItemId)
        {
            PlayerId = playerId;
            ShopItemId = shopItemId;
        }
    }
}
