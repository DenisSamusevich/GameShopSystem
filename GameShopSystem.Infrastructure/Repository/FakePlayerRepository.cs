using GameShopSystem.Domain.Entities;
using GameShopSystem.Domain.RepositoriesInterfaces;
using GameShopSystem.Infrastructure.Interfaces;

namespace GameShopSystem.Infrastructure.Repository
{
    internal class FakePlayerRepository : IPlayerRepository
    {
        private readonly List<Player> players;
        private readonly IFakeInventorySystem _inventorySystem;

        public FakePlayerRepository(IFakeInventorySystem inventorySystem)
        {
            _inventorySystem = inventorySystem;
            players = new List<Player>();
        }

        public Task<Player?> GetPlayerByIdAsync(Guid id)
        {
            return Task.FromResult(players.FirstOrDefault(p => p.Id.Equals(id)));
        }

        public async Task<bool> UpdateAsync(Player player)
        {
            var targetPlayer = players.FirstOrDefault(p => p.Id.Equals(player.Id));
            if (targetPlayer != null)
            {
                targetPlayer.BalanceGold = player.BalanceGold;
                targetPlayer.BalanceGems = player.BalanceGems;
                foreach (var purchaseEvent in player.PurchaseEvents)
                {
                    await _inventorySystem.AddItemAsync(purchaseEvent.PlayerId, purchaseEvent.ShopItemId);
                }
                foreach (var realMoneyPurchaseEvent in player.RealMoneyPurchaseEvents)
                {
                    await _inventorySystem.AddItemAsync(realMoneyPurchaseEvent.PlayerId, realMoneyPurchaseEvent.ShopItemId);
                }
                return await Task.FromResult(true);
            }
            return await Task.FromResult(false);
        }
    }
}
