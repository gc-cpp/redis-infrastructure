using Redis.OM.Searching;
using redis_infrastructure.Models;
using redis_infrastructure.RedisInfrastructure;

namespace redis_infrastructure.Services
{
    public class UserQueryBuilder
    {
        private IQueryable<User> _query;

        public UserQueryBuilder(RedisConnection redisConnection)
        {
            _query = redisConnection.GetRedisCollection<User>();
        }

        public UserQueryBuilder WithFirstName(string firstName)
        {
            if (string.IsNullOrWhiteSpace(firstName))
            {
                return this;
            }
            _query = _query.Where(x => x.FirstName == firstName);
            return this;
        }

        public UserQueryBuilder WithLastName(string lastName)
        {
            if (string.IsNullOrWhiteSpace(lastName))
            {
                return this;
            }
            _query = _query.Where(x => x.LastName == lastName);
            return this;
        }

        public UserQueryBuilder WithEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return this;
            }
            _query = _query.Where(x => x.Email == email);
            return this;
        }

        public UserQueryBuilder WithAgeUpperThen(int? age)
        {
            if (!age.HasValue)
            {
                return this;
            }
            _query = _query.Where(x => x.Age > age);
            return this;
        }

        public Task<IList<User>> BuildAsync()
        {
            var redisQuery = _query as IRedisCollection<User>;
            return redisQuery.ToListAsync();
        }
    }
}
