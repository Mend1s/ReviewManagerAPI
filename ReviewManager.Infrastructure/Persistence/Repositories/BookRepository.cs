using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ReviewManager.Core.Entities;
using ReviewManager.Core.Repositories;

namespace ReviewManager.Infrastructure.Persistence.Repositories;

public class BookRepository : IBookRepository
{
    private readonly ReviewDbContext _dbContext;
    private readonly ILogger<BookRepository> _logger;

    public BookRepository(
        ReviewDbContext dbContext,
        ILogger<BookRepository> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<Book> CreateAsync(Book book)
    {
        _logger.LogInformation("Criando um novo livro com título: {Title}", book.Title);
        await _dbContext.Books.AddAsync(book);
        await _dbContext.SaveChangesAsync();
        return book;
    }

    public async Task DeleteAsync(Book book)
    {
        _logger.LogInformation("Deletando livro com ID: {Id}", book.Id);
        _dbContext.Books.Remove(book);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<Book>> GetAllAsync()
    {
        _logger.LogInformation("Retornando todos os livros");
        return await _dbContext.Books.ToListAsync();
    }

    public async Task<Book> GetByIdAsync(int id)
    {
        _logger.LogInformation("Retornando livro com ID: {Id}", id);
        return await _dbContext.Books.SingleOrDefaultAsync(b => b.Id == id);
    }

    public async Task UpdateAsync(Book book)
    {
        _logger.LogInformation("Atualizando livro com ID: {Id}", book.Id);
        _dbContext.Books.Update(book);
        await _dbContext.SaveChangesAsync();
    }
}
