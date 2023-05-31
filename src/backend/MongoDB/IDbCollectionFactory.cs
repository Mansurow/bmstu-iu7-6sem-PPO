using Anticafe.DataAccess.DBModels;
using MongoDB.Driver;

namespace Anticafe.MongoDB;

public interface IDbCollectionFactory 
{
    public IMongoCollection<UserDbModel> GetUserCollection();
    public IMongoCollection<MenuDbModel> GetMenuCollection();
    public IMongoCollection<RoomDbModel> GetRoomCollection();
    public IMongoCollection<InventoryDbModel> GetInventoryCollection();
    public IMongoCollection<BookingDbModel> GetBookingCollection();
    public IMongoCollection<FeedbackDbModel> GetFeedbackCollection();
}
