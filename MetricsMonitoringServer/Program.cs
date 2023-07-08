using MetricsMonitoringServer.Controllers;
using MetricsMonitoringServer.Extensions;
using MetricsMonitoringServer.Services;
using Microsoft.Azure.Cosmos;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCustomSwaggerConfiguration();

builder.Services.AddSignalR();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddCustomAuthentication();
builder.Services.AddCustomAuthorization();

var cosmosDbConnectionString = builder.Configuration["CosmosDbConnectionString"]; //Its a secret 🤫

if (string.IsNullOrEmpty(cosmosDbConnectionString))
{
    Console.WriteLine("No CosmosDb connection string, switching to in-memory db");
    builder.Services.AddScoped<IRepository, FakeRepository>();
}
else
{
    builder.Services.AddScoped<IRepository, CosmosRepository>();

    builder.Services.AddSingleton(new CosmosClient(
        cosmosDbConnectionString,
        new CosmosClientOptions
        {
            SerializerOptions = new CosmosSerializationOptions
            {
                PropertyNamingPolicy = CosmosPropertyNamingPolicy.CamelCase
            }
        }
    ));
}


const string allowedOrigins = "AllowedOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: allowedOrigins,
        policy =>
        {
            policy
                .AllowAnyHeader()
                .AllowCredentials()
                .AllowAnyMethod()
                .WithOrigins("http://localhost:5500", "http://127.0.0.1:5500", "https://gentle-wave-0e686da03.3.azurestaticapps.net");
        });
});

var app = builder.Build();

app.UseCors(allowedOrigins);

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.MapHub<MetricsHub>("/hubs/metrics");

app.Run();