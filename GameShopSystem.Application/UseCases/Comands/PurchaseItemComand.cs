using Mediator;

namespace GameShopSystem.Application.UseCases.Comands
{
    public class PurchaseItemComand : ICommand<bool>
    {
        public Guid PlayerId { get; }
        public Guid ShopItemId { get; }

        public PurchaseItemComand(Guid playerId, Guid shopItemId)
        {
            PlayerId = playerId;
            ShopItemId = shopItemId;
        }
    }
}
