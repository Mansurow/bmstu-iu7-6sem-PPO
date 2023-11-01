using HotChocolate.Authorization;
using Portal.Common.Converter;
using Portal.Common.Dto.User;
using Portal.Common.Enums;
using Portal.Services.UserService;

namespace Portal.Graphql.Queries;

[ExtendObjectType("Query")]
public class UserQuery
{
    private readonly ILogger<UserQuery> _logger;

    public UserQuery(ILogger<UserQuery> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Получить всех пользователей.
    /// </summary>
    /// <param name="userService"></param>
    /// <returns>Список данных пользователей.</returns>
    [GraphQLName("GetUsers")]
    [Authorize(Roles = new []{nameof(Role.Administrator)})]
    public async Task<IEnumerable<User>> GetUsers([Service(ServiceKind.Resolver)] IUserService userService)
    {
        var users = await userService.GetAllUsersAsync();

        return users.Select(UserConverter.ConvertCoreToDtoModel);
    }

    /// <summary>
    /// Получить данные пользователя.
    /// </summary>
    /// <param name="userService"></param>
    /// <param name="id" example="b0ae1a74-7e95-4816-9b60-320707b77579">Идентификатор пользователя.</param>
    /// <returns>Данных пользователя.</returns>
    [GraphQLName("GetUser")]
    [Authorize]
    public async Task<User> GetUser([Service(ServiceKind.Resolver)] IUserService userService, Guid id)
    {
        var user = await userService.GetUserByIdAsync(id);

        return UserConverter.ConvertCoreToDtoModel(user);
    }
}