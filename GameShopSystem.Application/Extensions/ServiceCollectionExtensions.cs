using Microsoft.Extensions.DependencyInjection;

namespace GameShopSystem.Application.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection serviceCollection)
        {
            return serviceCollection.AddMediator((m) =>
            {
                m.Assemblies = [typeof(ServiceCollectionExtensions).Assembly];
            });
        }
    }
}
