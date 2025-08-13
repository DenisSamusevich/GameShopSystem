using GameShopSystem.Domain.Entities;

namespace GameShopSystem.Domain.RepositoriesInterfaces
{
    public interface IPlayerRepository
    {
        Task<Player?> GetPlayerByIdAsync(Guid id);
        Task<bool> UpdateAsync(Player player);
    }
}
