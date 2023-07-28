using Microsoft.EntityFrameworkCore;
using Portal.Common.Converter;
using Portal.Common.Models;
using Portal.Database.Context;
using Portal.Database.Models;
using Portal.Database.Repositories.Interfaces;

namespace Portal.Database.Repositories.NpgsqlRepositories;

public class UserRepository: BaseRepository, IUserRepository
{
    private readonly PortalDbContext _context;

    public UserRepository(PortalDbContext context)
    {
        _context = context;
    }
    
    public Task<List<User>> GetAllUsersAsync()
    {
        return _context.Users
            .Select(u => UserConverter.ConvertDbModelToAppModel(u)!)
            .ToListAsync();
    }

    public async Task<User> GetUserByIdAsync(Guid userId)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

        return UserConverter.ConvertDbModelToAppModel(user);
    }

    public async Task<User> GetUserByEmailAsync(string email)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

        return UserConverter.ConvertDbModelToAppModel(user);
    }

    public async Task InsertUserAsync(User user)
    {
        var userDb = UserConverter.ConvertAppModelToDbModel(user);
        
        await _context.Users.AddAsync(userDb);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateUserAsync(User user)
    {
        var userDb = UserConverter.ConvertAppModelToDbModel(user);
        
        _context.Users.Update(userDb);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteUserAsync(Guid userId)
    {
        var user = await _context.Users.FirstAsync(u => u.Id == userId);
        
        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
    }
}