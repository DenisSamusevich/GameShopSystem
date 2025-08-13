using GameShopSystem.Domain.Entities;

namespace GameShopSystem.Domain.RepositoriesInterfaces
{
    public interface IShopItemRepository
    {
        Task<ShopItem?> GetShopItemByIdAsync(Guid id);
        Task<IReadOnlyList<ShopItem>> GetAllShopItemAsync();
    }
}
