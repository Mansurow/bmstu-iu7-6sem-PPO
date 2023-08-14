namespace Portal.Common.Models;

public class AuthorizationResponse
{
    public AuthorizationResponse(Guid userId, string accessToken)
    {
        UserId = userId;
        AccessToken = accessToken;
    }

    public Guid UserId { get; set; }
    
    public string AccessToken { get; set; }
}