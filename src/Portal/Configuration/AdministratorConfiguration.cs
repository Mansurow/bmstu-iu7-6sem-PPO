namespace Portal.Configuration;

public class AdministratorConfiguration 
{
    public AdministratorConfiguration(string login, string password)
    {
        Login = login;
        Password = password;
    }

    public string Login { get; set; }
    
    public string Password { get; set; }
}