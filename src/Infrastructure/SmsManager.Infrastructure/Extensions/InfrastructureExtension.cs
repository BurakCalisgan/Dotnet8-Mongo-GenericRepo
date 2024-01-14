using Microsoft.Extensions.DependencyInjection;
using SmsManager.Domain.Repository;
using SmsManager.Infrastructure.Repository;

namespace SmsManager.Infrastructure.Extensions;

public static class InfrastructureExtension
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        return services
            .AddSingleton<ISmsRepository, SmsRepository>();
    }
}