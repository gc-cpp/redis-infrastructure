using Redis.OM.Modeling;
using redis_infrastructure.Models;
using redis_infrastructure.RedisInfrastructure;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
ConfigureOptions(builder.Services, builder.Configuration);
ConfigureRedis(builder.Services, builder.Configuration);

var app = builder.Build();
await EnsureRedisConnectionSucssessfullAsync(app);
app.MapControllers();
app.Run();

void ConfigureOptions(IServiceCollection services, IConfiguration configuration)
{
    services.Configure<RedisSettings>(configuration.GetSection(nameof(RedisSettings)));
}

void ConfigureRedis(IServiceCollection services, IConfiguration configuration)
{
    services.AddSingleton<RedisConnection>();

    DocumentAttribute.RegisterIdGenerationStrategy(nameof(GuidStrategy), new GuidStrategy());
}

async Task EnsureRedisConnectionSucssessfullAsync(IApplicationBuilder app)
{
    using var scope = app.ApplicationServices.CreateScope();
    var redis = scope.ServiceProvider.GetRequiredService<RedisConnection>();
    await redis.TestConnectionAsync();

    await redis.CreateIndexAsync<User>();
}
