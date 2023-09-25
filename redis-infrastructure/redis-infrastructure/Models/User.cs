using Redis.OM.Modeling;
using redis_infrastructure.RedisInfrastructure;

namespace redis_infrastructure.Models
{
    [Document(StorageType = StorageType.Json, IdGenerationStrategyName = nameof(GuidStrategy))]
    public class User
    {
        [RedisIdField]
        public string? UserId { get; set; }
        [Indexed]
        public string FirstName { get; set; }
        [Indexed]
        public string LastName { get; set; }
        public string? Email { get; set; }
        [Indexed(Sortable = true)]
        public int Age { get; set; }
    }
}
