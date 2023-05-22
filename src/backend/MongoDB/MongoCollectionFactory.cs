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

    public IMongoCollection<UserDbModel> GetUserCollection() 
    {
        var dbName = _config.GetSection("MongoDb").GetSection("DatabaseName").Value;
        var db = _mongoClient.GetDatabase(dbName);

        var collectionName = _config.GetSection("MongoDb").GetSection("UserCollectionName").Value;
        return db.GetCollection<UserDbModel>(collectionName);
    }

    public IMongoCollection<MenuDbModel> GetMenuCollection()
    {
        var dbName = _config.GetSection("MongoDb").GetSection("DatabaseName").Value;
        var db = _mongoClient.GetDatabase(dbName);

        var collectionName = _config.GetSection("MongoDb").GetSection("MenuCollectionName").Value;
        return db.GetCollection<MenuDbModel>(collectionName);
    }

    public IMongoCollection<RoomDbModel> GetRoomCollection()
    {
        var dbName = _config.GetSection("MongoDb").GetSection("DatabaseName").Value;
        var db = _mongoClient.GetDatabase(dbName);

        var collectionName = _config.GetSection("MongoDb").GetSection("RoomCollectionName").Value;
        return db.GetCollection<RoomDbModel>(collectionName);
    }

    public IMongoCollection<InventoryDbModel> GetInventoryCollection()
    {
        var dbName = _config.GetSection("MongoDb").GetSection("DatabaseName").Value;
        var db = _mongoClient.GetDatabase(dbName);

        var collectionName = _config.GetSection("MongoDb").GetSection("InventoryCollectionName").Value;
        return db.GetCollection<InventoryDbModel>(collectionName);
    }

    public IMongoCollection<BookingDbModel> GetBookingCollection()
    {
        var dbName = _config.GetSection("MongoDb").GetSection("DatabaseName").Value;
        var db = _mongoClient.GetDatabase(dbName);

        var collectionName = _config.GetSection("MongoDb").GetSection("BookingCollectionName").Value;
        return db.GetCollection<BookingDbModel>(collectionName);
    }

    public IMongoCollection<FeedbackDbModel> GetFeedbackCollection()
    {
        var dbName = _config.GetSection("MongoDb").GetSection("DatabaseName").Value;
        var db = _mongoClient.GetDatabase(dbName);

        var collectionName = _config.GetSection("MongoDb").GetSection("FeedbackCollectionName").Value;
        return db.GetCollection<FeedbackDbModel>(collectionName);
    }
}
