using Microsoft.Extensions.DependencyInjection;
using SmsManager.Application.Services.Abstractions;
using SmsManager.Application.Services.Implementations;

namespace SmsManager.Application.Extensions;

public static class ApplicationExtension
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        return services.AddScoped<ISmsService, SmsService>();
    }
}