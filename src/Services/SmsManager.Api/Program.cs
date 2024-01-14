using System.Text.Json.Serialization;
using SmsManager.Application.Extensions;
using SmsManager.Infrastructure.Extensions;
using SmsManager.Infrastructure.Settings;

var builder = WebApplication.CreateBuilder(args);

// Applications injection
builder.Services.AddApplication();

// Infrastructure injection
builder.Services.AddInfrastructure();

// Http Clients injection
builder.Services.AddHttpClient("SmsApiClient", c =>
{
    c.BaseAddress = new Uri(builder.Configuration.GetValue<string>("SmsProvider:SmsApiBaseUrl")!);
});

// Configuration - Settings injection
builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection("MongoDbSettings"));


builder.Services.AddControllers().AddJsonOptions(options =>
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

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
