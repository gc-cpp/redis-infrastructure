using redis_infrastructure.Models;
using redis_infrastructure.RedisInfrastructure;

namespace redis_infrastructure.Services
{
    public class UserQueryBuilder
    {
        private IEnumerable<User> _query;

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
            _query = _query.Where(x => string.Equals(x.FirstName, firstName, StringComparison.OrdinalIgnoreCase));
            return this;
        }

        public UserQueryBuilder WithLastName(string lastName)
        {
            if (string.IsNullOrWhiteSpace(lastName))
            {
                return this;
            }
            _query = _query.Where(x => string.Equals(x.LastName, lastName, StringComparison.OrdinalIgnoreCase));
            return this;
        }

        public UserQueryBuilder WithEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return this;
            }
            _query = _query.Where(x => string.Equals(x.Email, email, StringComparison.OrdinalIgnoreCase));
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

        public List<User> Build()
        {
            return _query.ToList();
        }
    }
}
