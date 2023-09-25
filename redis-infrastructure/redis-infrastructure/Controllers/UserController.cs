using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using redis_infrastructure.Models;
using redis_infrastructure.RedisInfrastructure;
using redis_infrastructure.Requests;
using redis_infrastructure.Services;

namespace redis_infrastructure.Controllers
{
    [ApiController]
    [Route("{controller}")]
    public class UserController : ControllerBase
    {
        private readonly RedisConnection _connection;
        private readonly IOptionsSnapshot<RedisSettings> _redisSettings;
        private readonly ILogger _logger;

        public UserController(
            RedisConnection redisConnection,
            IOptionsSnapshot<RedisSettings> redisSettings,
            ILogger<UserController> userController)
        {
            _connection = redisConnection;
            _redisSettings = redisSettings;
            _logger = userController;
        }

        [HttpPost]
        public async Task<IActionResult> AddUserAsync([FromBody] User user, CancellationToken cancellationToken)
        {
            var id = await _connection.GetRedisCollection<User>().InsertAsync(user, _redisSettings.Value.UserCacheTime);
            _logger.LogInformation($"Insert user with id = {id}");
            return StatusCode(StatusCodes.Status201Created, id);
        }

        [HttpGet("/{id}")]
        public async Task<IActionResult> GetUserAsync([FromQuery] string id, CancellationToken cancellationToken)
        {
            var user = await _connection.GetRedisCollection<User>().FindByIdAsync(id);
            if (user == null)
            {
                _logger.LogInformation($"Not found user with id = {id}");
                return NotFound();
            }
            _logger.LogInformation($"Get user with id = {id}");
            return Ok(user);
        }

        [HttpPost("/find")]
        public async Task<IActionResult> FindUsersAsync([FromBody] UserSearchRequest userSearchRequest, CancellationToken cancellationToken)
        {
            var builder = new UserQueryBuilder(_connection);
            var users = builder
                .WithFirstName(userSearchRequest.FirstName)
                .WithLastName(userSearchRequest.LastName)
                .WithEmail(userSearchRequest.Email)
                .WithAgeUpperThen(userSearchRequest.Age)
                .Build();
            _logger.LogInformation("Find users");
            return Ok(users);
        }

        [HttpDelete]
        public async Task<IActionResult> RemoveUserAsync(string id, CancellationToken cancellationToken)
        {
            var user = await _connection.GetRedisCollection<User>().FindByIdAsync(id);
            if (user == null)
            {
                _logger.LogInformation($"Not found user with id = {id}");
                return NotFound();
            }
            await _connection.GetRedisCollection<User>().DeleteAsync(user);
            _logger.LogInformation($"Delete user with id = {id}");
            return NoContent();
        }
    }
}
