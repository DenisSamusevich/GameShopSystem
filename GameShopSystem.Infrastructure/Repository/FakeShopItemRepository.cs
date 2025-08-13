using GameShopSystem.Domain.Entities;
using GameShopSystem.Domain.RepositoriesInterfaces;

namespace GameShopSystem.Infrastructure.Repository
{
    internal class FakeShopItemRepository : IShopItemRepository
    {
        private readonly List<ShopItem> _shopItems;
        public FakeShopItemRepository()
        {
            _shopItems = new List<ShopItem>();
        }

        public Task<IReadOnlyList<ShopItem>> GetAllShopItemAsync()
        {
            return Task.FromResult<IReadOnlyList<ShopItem>>(_shopItems.ToList());
        }

        public Task<ShopItem?> GetShopItemByIdAsync(Guid id)
        {
            return Task.FromResult(_shopItems.FirstOrDefault(s => s.Id.Equals(s.Id)));
        }
    }
}
