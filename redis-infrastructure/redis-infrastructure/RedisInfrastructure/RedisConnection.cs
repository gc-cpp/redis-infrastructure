using Microsoft.Extensions.Options;
using Redis.OM;
using Redis.OM.Searching;
using StackExchange.Redis;

namespace redis_infrastructure.RedisInfrastructure
{
    public class RedisConnection
    {
        private readonly Lazy<RedisConnectionProvider> _redisConnectionProvider;

        public RedisConnection(IOptions<RedisSettings> redisSettings)
        {
            _redisConnectionProvider = new Lazy<RedisConnectionProvider>(
                    () => new RedisConnectionProvider(ConfigurationOptions.Parse(redisSettings.Value.ConnectionString))
                );
        }

        public IRedisCollection<T> GetRedisCollection<T>() where T : notnull
        {
            return _redisConnectionProvider.Value.RedisCollection<T>();
        }

        public Task CreateIndexAsync<T>()
        {
            return _redisConnectionProvider.Value.Connection.CreateIndexAsync(typeof(T));
        }

        public async Task TestConnectionAsync()
        {
            try
            {
                await _redisConnectionProvider.Value.Connection.ExecuteAsync("ACL", "WHOAMI").ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw new Exception("Cannot create connection to Redis", ex);
            }
        }
    }
}
