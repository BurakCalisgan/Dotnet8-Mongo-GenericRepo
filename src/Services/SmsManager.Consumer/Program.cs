using MassTransit;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Serilog;
using SmsManager.Application.Extensions;
using SmsManager.Consumer.Consumers;
using SmsManager.Infrastructure.Extensions;
using SmsManager.Infrastructure.Settings;

var builder = WebApplication.CreateBuilder(args);

//Health Checks
builder.Services
    .AddDefaultHealthCheck()
    .AddMongoDbHealthCheck(builder.Configuration)
    .AddRabbitmqHealthCheck(builder.Configuration);

// Applications injection
builder.Services.AddApplication();

// Infrastructure injection
builder.Services.AddInfrastructure();

// HttpContextAccessor injection
builder.Services.AddHttpContextAccessor();

// Http Clients injection
builder.Services.AddHttpClient("SmsApiClient", c =>
{
    c.BaseAddress = new Uri(builder.Configuration.GetValue<string>("SmsProvider:SmsApiBaseUrl") ?? throw new
        InvalidOperationException());
});

// Configuration - Settings injection
builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection("MongoDbSettings"));

// Serilog injection
builder.Host.UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration));

// Message Broker injection
var rabbitmqSettings = builder.Configuration.GetSection("RabbitmqConfiguration");
builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(rabbitmqSettings["HostName"], t =>
        {
            t.Username(rabbitmqSettings["UserName"]);
            t.Password(rabbitmqSettings["Password"]);
        });
        cfg.ConfigureEndpoints(context, KebabCaseEndpointNameFormatter.Instance);
        cfg.PrefetchCount = 8;
        cfg.UseMessageRetry(r => r.Interval(5, TimeSpan.FromMilliseconds(300)));
        cfg.ConfigureJsonSerializerOptions(options =>
        {
            options.MaxDepth = int.MaxValue;
            return options;
        });
    });
    
    x.AddConsumers(typeof(SendSmsConsumer).Assembly);
});

var app = builder.Build();

// HealthCheck middleware
app.UseHealthChecks("/health-alive", new HealthCheckOptions
{
    Predicate = check => check.Tags.Contains("alive"),
    ResponseWriter = async (context, report) =>
        await HealthCheckExtension.ResponseWriter(context, report, builder.Environment.EnvironmentName)
});
app.UseHealthChecks("/health-ready", new HealthCheckOptions
{
    Predicate = check => check.Tags.Contains("ready"),
    ResponseWriter = async (context, report) =>
        await HealthCheckExtension.ResponseWriter(context, report, builder.Environment.EnvironmentName)
});

app.Run();