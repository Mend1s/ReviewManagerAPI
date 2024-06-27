using ReviewManager.Core.Entities;

namespace ReviewManager.Core.Repositories;

public interface IBookRepository
{
    Task<IEnumerable<Book>> GetAllAsync();
    Task<Book> GetByIdAsync(int id);
    Task<Book> CreateAsync(Book book);
    Task UpdateAsync(Book book);
    Task DeleteAsync(Book book);
}
