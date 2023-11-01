using HotChocolate.Authorization;
using Portal.Common.Converter;
using Portal.Common.Dto;
using Portal.Common.Dto.User;
using Portal.Services.OauthService;

namespace Portal.Graphql.Mutations;

[ExtendObjectType("Mutation")]
public class UserMutation
{
    private readonly ILogger<UserMutation> _logger;

    public UserMutation(ILogger<UserMutation> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Зарегистрировать пользователя.
    /// </summary>
    /// <param name="oauthService"></param>
    /// <param name="createUser">Данные для регистрации</param>
    /// <returns>Токен JWT.</returns>
    [GraphQLName("SignUp")]
    [AllowAnonymous]
    public async Task<AuthorizationResponse> SignUp([Service(ServiceKind.Resolver)] IOauthService oauthService, CreateUser createUser)
    {
        var user = UserConverter.ConvertDtoToCoreModel(createUser);

        await oauthService.Registrate(user, createUser.Password);

        var token = oauthService.GenerateJwt(user);
            
        return new AuthorizationResponse(user.Id, token);
    }
    
    /// <summary>
    /// Авторизовать пользователя.
    /// </summary>
    /// <param name="oauthService"></param>
    /// <param name="login">Логин (Email).</param>
    /// <param name="password">Пароль.</param>
    /// <returns>Токен JWT.</returns>
    [GraphQLName("SignIn")]
    [AllowAnonymous]
    public async Task<AuthorizationResponse> SignIn([Service(ServiceKind.Resolver)] IOauthService oauthService, 
        string login, string password)
    {
        var user = await oauthService.LogIn(login, password);

        var token = oauthService.GenerateJwt(user);
            
        return new AuthorizationResponse(user.Id, token);
    }
}