using Redis.OM;

namespace redis_infrastructure.RedisInfrastructure
{
    public class GuidStrategy : IIdGenerationStrategy
    {
        public string GenerateId() => Guid.NewGuid().ToString();
    }
}
