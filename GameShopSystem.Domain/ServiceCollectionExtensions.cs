using GameShopSystem.Domain.Services;
using GameShopSystem.Domain.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace GameShopSystem.Domain
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDomainService(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IPurchaseService, PurchaseService>()
                .AddSingleton<IDiscountService, DiscountService>()
                .AddSingleton<IRealMoneyPurchaseService, RealMoneyPurchaseService>();
            return serviceCollection;
        }
    }
}
