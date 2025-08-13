namespace GameShopSystem.Infrastructure.Interfaces
{
    internal interface IFakeInventorySystem
    {
        Task AddItemAsync(Guid playerId, Guid shopItemId);
    }
}
