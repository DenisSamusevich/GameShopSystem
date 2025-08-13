using GameShopSystem.Domain.Entities;
using GameShopSystem.Domain.RepositoriesInterfaces;

namespace GameShopSystem.Infrastructure.Repository
{
    public class FakeDiscountRepository : IDiscountRepository
    {
        private readonly List<Discount> _discounts;

        public FakeDiscountRepository()
        {
            _discounts = new List<Discount>();
        }

        public Task<IReadOnlyList<Discount>> GetAllDiscountsAsync()
        {
            return Task.FromResult<IReadOnlyList<Discount>>(_discounts.ToList());
        }

        public Task<Discount?> GetDiscountByIdAsync(Guid id)
        {
            return Task.FromResult(_discounts.FirstOrDefault(d => d.Id.Equals(id)));
        }
    }
}
