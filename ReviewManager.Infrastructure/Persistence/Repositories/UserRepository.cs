using Microsoft.EntityFrameworkCore;
using ReviewManager.Core.Entities;
using ReviewManager.Core.Repositories;

namespace ReviewManager.Infrastructure.Persistence.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ReviewDbContext _dbContext;
    public UserRepository(ReviewDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<User> CreateAsync(User user)
    {
        await _dbContext.Users.AddAsync(user);
        await _dbContext.SaveChangesAsync();
        return user;
    }

    public async Task DeleteAsync(User user)
    {
        _dbContext.Users.Remove(user);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        return await _dbContext.Users.ToListAsync();
    }

    public async Task<User> GetByIdAsync(int id)
    {
        return await _dbContext.Users.SingleOrDefaultAsync(u => u.Id == id);
    }

    public async Task UpdateAsync(User user)
    {
        _dbContext.Users.Update(user);
        await _dbContext.SaveChangesAsync();
    }
}
