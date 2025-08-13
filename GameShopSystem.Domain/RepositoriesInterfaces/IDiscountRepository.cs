using GameShopSystem.Domain.Entities;

namespace GameShopSystem.Domain.RepositoriesInterfaces
{
    public interface IDiscountRepository
    {
        Task<Discount?> GetDiscountByIdAsync(Guid id);
        Task<IReadOnlyList<Discount>> GetAllDiscountsAsync();
    }
}
