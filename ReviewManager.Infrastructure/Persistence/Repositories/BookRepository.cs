using Microsoft.EntityFrameworkCore;
using ReviewManager.Core.Entities;
using ReviewManager.Core.Repositories;

namespace ReviewManager.Infrastructure.Persistence.Repositories;

public class BookRepository : IBookRepository
{
    private readonly ReviewDbContext _dbContext;

    public BookRepository(ReviewDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Book> CreateAsync(Book book)
    {
        await _dbContext.Books.AddAsync(book);
        await _dbContext.SaveChangesAsync();
        return book;
    }

    public async Task DeleteAsync(Book book)
    {
        _dbContext.Books.Remove(book);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<Book>> GetAllAsync()
    {
        return await _dbContext.Books.ToListAsync();
    }

    public async Task<Book> GetByIdAsync(int id)
    {
        return await _dbContext.Books.SingleOrDefaultAsync(b => b.Id == id);
    }

    public async Task UpdateAsync(Book book)
    {
        _dbContext.Books.Update(book);
        await _dbContext.SaveChangesAsync();
    }
}
