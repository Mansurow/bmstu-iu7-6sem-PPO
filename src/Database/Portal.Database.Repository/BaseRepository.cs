namespace Portal.Database.Models;

public class BaseRepository
{
    protected readonly string RepositoryName;

    public BaseRepository()
    {
        RepositoryName = GetType().Name;
    }
}
