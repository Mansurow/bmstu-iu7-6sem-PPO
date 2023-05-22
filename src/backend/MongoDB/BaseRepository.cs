using MongoDB.Driver;

namespace Anticafe.MongoDB;

public class BaseRepository
{
    private readonly string _repositoryName;
    public BaseRepository()
    {
        _repositoryName = GetType().Name;
    }
}
