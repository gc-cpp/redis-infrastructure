namespace redis_infrastructure.RedisInfrastructure
{
    public record RedisSettings
    {
        public string ConnectionString { get; set; }
        public TimeSpan UserCacheTime { get; set; } = TimeSpan.FromSeconds(1);
    }
}
