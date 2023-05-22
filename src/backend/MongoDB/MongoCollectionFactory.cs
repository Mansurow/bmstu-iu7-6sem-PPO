using Anticafe.DataAccess.DBModels;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Anticafe.MongoDB;

public class MongoCollectionFactory: IDbCollectionFactory
{
    private IMongoClient _mongoClient { get; set; }

    public IConfiguration _config;

    public MongoCollectionFactory(IMongoClient client, IConfiguration config) 
    {
        _mongoClient = client;
        _config = config;
    }

    public IMongoCollection<UserDbModel> getUserCollection() 
    {
        var dbName = _config.GetSection("MongoDb").GetSection("DatabaseName").Value;
        var db = _mongoClient.GetDatabase(dbName);

        var collectionName = _config.GetSection("MongoDb").GetSection("UserCollectionName").Value;
        return db.GetCollection<UserDbModel>(collectionName);
    }

    public IMongoCollection<MenuDbModel> getMenuCollection()
    {
        var dbName = _config.GetSection("MongoDb").GetSection("DatabaseName").Value;
        var db = _mongoClient.GetDatabase(dbName);

        var collectionName = _config.GetSection("MongoDb").GetSection("MenuCollectionName").Value;
        return db.GetCollection<MenuDbModel>(collectionName);
    }
}
