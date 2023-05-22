using Anticafe.DataAccess.DBModels;
using MongoDB.Driver;

namespace Anticafe.MongoDB;

public interface IDbCollectionFactory 
{
    public IMongoCollection<UserDbModel> getUserCollection();
    public IMongoCollection<MenuDbModel> getMenuCollection();
}
