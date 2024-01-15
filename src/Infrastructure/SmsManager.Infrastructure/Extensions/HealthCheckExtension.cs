using System.Net.Mime;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace SmsManager.Infrastructure.Extensions;

public static class HealthCheckExtension
{
    public static IHealthChecksBuilder AddDefaultHealthCheck(this IServiceCollection services)
    {
        return services.AddHealthChecks()
            .AddCheck("ApplicationHealth", () =>
                HealthCheckResult.Healthy("Healthy."), tags: new[] { "alive" });
    }

    public static IHealthChecksBuilder AddMongoDbHealthCheck(this IHealthChecksBuilder builder,
        IConfiguration configuration)
    {
        return builder
            .AddMongoDb(configuration.GetValue<string>("MongoDbSettings:ConnectionString")!,
                name: "MongoDb", tags: new[] { "ready" });
    }

    public static IHealthChecksBuilder AddRabbitmqHealthCheck(this IHealthChecksBuilder builder,
        IConfiguration configuration)
    {
        var rabbitmqConfiguration = configuration.GetSection("RabbitmqConfiguration");
        var rabbitmqConnectionString =
            $"amqp://{rabbitmqConfiguration["UserName"]}:{rabbitmqConfiguration["Password"]}@{rabbitmqConfiguration["HostName"]}";

        return builder.AddRabbitMQ(rabbitmqConnectionString, name: "RabbitMq", tags: new[] { "ready" });
    }
    
    public static async Task ResponseWriter(HttpContext context, HealthReport report, string environmentName)
    {
        var result = JsonSerializer.Serialize(new
        {
            environment = environmentName,
            status = report.Status.ToString(),
            checks = report.Entries.Select(entry => new
            {
                name = entry.Key,
                status = entry.Value.Status.ToString()
            })
        });
        context.Response.ContentType = MediaTypeNames.Application.Json;
        await context.Response.WriteAsync(result);
    }
}