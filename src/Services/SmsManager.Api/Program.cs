using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Serilog;
using SmsManager.Application.Extensions;
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

// Http Clients injection
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient("SmsApiClient", c =>
{
    c.BaseAddress = new Uri(builder.Configuration.GetValue<string>("SmsProvider:SmsApiBaseUrl")!);
});

// Configuration - Settings injection
builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection("MongoDbSettings"));

// Serilog injection
builder.Host.UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration));

builder.Services.AddControllers().AddJsonOptions(options =>
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();
