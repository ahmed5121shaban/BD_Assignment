using BD_Services;
using Hangfire;
using Hangfire.MemoryStorage;
using Microsoft.OpenApi.Models;
using Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpClient<IIpApiService,IpApiService>();
builder.Services.AddScoped<BlockedCountriesService>();
builder.Services.AddScoped<IBlockedCountryRepository, BlockedCountryRepository>();

builder.Services.AddHangfire(config =>
{
    config.UseMemoryStorage();
});
builder.Services.AddHangfireServer();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Blocked Countries API",
        Version = "v1",
        Description = "API for managing blocked countries and IP validation"
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseHangfireDashboard();
RecurringJob.AddOrUpdate<BackGroundTaskService>(
    "RemoveExpiredTemporalBlocks",
    service => service.RemoveExpiredTemporalBlocks(),
    "*/5 * * * *" // Every 5 minutes
);
// Enable Swagger UI in development
app.UseDeveloperExceptionPage();
app.UseStaticFiles();
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Blocked Countries API v1");
});

app.UseAuthorization();

app.MapControllers();

app.Run();
