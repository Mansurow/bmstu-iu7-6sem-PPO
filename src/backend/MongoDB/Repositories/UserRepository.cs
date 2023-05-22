using Anticafe.DataAccess.DBModels;
using Anticafe.DataAccess.Exceptions;
using Anticafe.DataAccess.IRepositories;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;

namespace Anticafe.MongoDB.Repositories;

public class UserRepository: BaseRepository, IUserRepository 
{
    private readonly IMongoCollection<UserDbModel> _userCollection;

    public UserRepository(IDbCollectionFactory collections)
    {
        _userCollection = collections.GetUserCollection();
    }

    public async Task<List<UserDbModel>> GetAllUsersAsync()
    {
        var users = await _userCollection.Find(_ => true).ToListAsync();
        return users;
    }

    public async Task<UserDbModel> GetUserByIdAsync(int userId)
    {
        var user = await _userCollection.Find(u => u.Id == userId).FirstOrDefaultAsync();
        if (user is null)
        {
            throw new UserNotFoundByIdException($"User not found with id: {userId}");
        }
        return user;
    }

    public async Task<UserDbModel> GetUserByEmailAsync(string email)
    {
        var user = await _userCollection.Find(u => u.Email == email).FirstOrDefaultAsync();
        return user;
    }

    public async Task InsertUserAsync(UserDbModel createUser)
    {
        try
        {
            var tmp = _userCollection.Find(_ => true).ToList();
            if (tmp.Count > 0)
                createUser.Id = tmp.Select(x => x.Id).Max() + 1;
            else
                createUser.Id = 1;

            await _userCollection.InsertOneAsync(createUser);
        }
        catch
        {
            throw new UserCreateException("User not create");
        }
    }

    public async Task UpdateUserAsync(UserDbModel updateUser)
    {
        try
        {
            var filter = Builders<UserDbModel>.Filter.Eq(u => u.Id, updateUser.Id);
            var update = Builders<UserDbModel>.Update.Set(u => u.Name, updateUser.Name)
                                                     .Set(u => u.Surname, updateUser.Name)
                                                     .Set(u => u.FirstName, updateUser.FirstName)
                                                     .Set(u => u.Gender, updateUser.Gender)
                                                     .Set(u => u.Birthday, updateUser.Birthday)
                                                     .Set(u => u.Email, updateUser.Email)
                                                     .Set(u => u.Phone, updateUser.Phone);
            await _userCollection.UpdateOneAsync(filter, update);
        }
        catch
        {
            throw new UserUpdateException($"User not update with id: {updateUser.Id}");
        }
    }

    public async Task DeleteUserAsync(int userId)
    {
        try
        {
            var filter = Builders<UserDbModel>.Filter.Lt(u => u.Id, userId);
            await _userCollection.DeleteOneAsync(filter);
        }
        catch
        {
            throw new UserDeleteException($"User not delete with id: {userId}");
        }
    }
}
