using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReviewManager.Application.InputModels;
using ReviewManager.Application.Services.Interfaces;
using ReviewManager.Application.ViewModels;
using ReviewManager.Core.Entities;
using ReviewManager.Infrastructure.Persistence;

namespace ReviewManager.Application.Services.Implementations;

public class BookService : IBookService
{
    private readonly ReviewDbContext _dbContext;
    public BookService(ReviewDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Book> CreateBook(CreateBookInputModel createBookInputModel)
    {
        var filePath = Path.Combine("Storage", createBookInputModel.ImageUrl.FileName);

        using Stream fileStream = new FileStream(filePath, FileMode.Create);
        createBookInputModel.ImageUrl.CopyTo(fileStream);

        var book = new Book(
            createBookInputModel.Title,
            createBookInputModel.Description,
            createBookInputModel.ISBN,
            createBookInputModel.Author,
            createBookInputModel.Publisher,
            createBookInputModel.Genre,
            createBookInputModel.YearOfPublication,
            createBookInputModel.NumberOfPages,
            filePath);

        await _dbContext.Books.AddAsync(book);

        await _dbContext.SaveChangesAsync();

        return book;
    }

    public async Task<bool> DeleteBook(int id)
    {
        var book = _dbContext.Books.SingleOrDefault(b => b.Id == id);

        if (book is null) throw new Exception("Livro não encontrado.");

        _dbContext.Books.Remove(book);

        await _dbContext.SaveChangesAsync();

        return true;
    }

    public async Task<List<BookViewModel>> GetAllBooks()
    {
        var books = await _dbContext.Books.ToListAsync();

        if (books is null) throw new Exception("Livros não encontrado.");

        var bookViewModel = books.Select(book => new BookViewModel
        {
            Id = book.Id,
            Title = book.Title,
            Description = book.Description,
            ISBN = book.ISBN,
            Author = book.Author,
            Publisher = book.Publisher,
            Genre = book.Genre,
            YearOfPublication = book.YearOfPublication,
            NumberOfPages = book.NumberOfPages,
            CreateDate = book.CreateDate,
            AverageGrade = book.AverageGrade,
            ImageUrl = book.ImageUrl
        }).ToList();

        return bookViewModel;
    }

    public async Task<BookViewModel> GetBookById(int id)
    {
        var book = await _dbContext.Books.SingleOrDefaultAsync(b => b.Id == id);

        if (book is null) throw new Exception("Livro não encontrado.");

        var bookViewModel = new BookViewModel
        {
            Id = book.Id,
            Title = book.Title,
            Description = book.Description,
            ISBN = book.ISBN,
            Author = book.Author,
            Publisher = book.Publisher,
            Genre = book.Genre,
            YearOfPublication = book.YearOfPublication,
            NumberOfPages = book.NumberOfPages,
            CreateDate = book.CreateDate,
            ImageUrl = book.ImageUrl,
            AverageGrade = book.AverageGrade
        };

        return bookViewModel;
    }

    public async Task<Book> UpdateBook(int id, UpdateBookInputModel updateBookInputModel)
    {
        var book = _dbContext.Books.SingleOrDefault(b => b.Id == id);

        if (book is null) throw new Exception("Livro não encontrado.");

        book.UpdateBook(
            updateBookInputModel.Title,
            updateBookInputModel.Description,
            updateBookInputModel.ISBN,
            updateBookInputModel.Author,
            updateBookInputModel.Publisher,
            updateBookInputModel.Genre,
            updateBookInputModel.YearOfPublication,
            updateBookInputModel.NumberOfPages,
            updateBookInputModel.AverageGrade);

        _dbContext.Update(book);

        await _dbContext.SaveChangesAsync();

        return book;
    }

    public async Task<FileResult> DownloadPhotoBookAsync(int id)
    {
        // Busca o livro no banco
        var book = await _dbContext.Books.SingleOrDefaultAsync(b => b.Id == id);

        // Verifica se existe o livro no banco
        if (book == null) throw new ArgumentException("Livro não encontrado");
        
        // Verifica se o arquivo existe
        if (!File.Exists(book.ImageUrl)) throw new FileNotFoundException("Arquivo de imagem não encontrado");
        
        // Ler os bytes do arquivo
        var dataBytes = await File.ReadAllBytesAsync(book.ImageUrl);

        // Determinar o tipo MIME da imagem (depende do formato real da imagem)
        var contentType = "image/png";

        // Retornar o conteúdo do arquivo como um FileContentResult
        return new FileContentResult(dataBytes, contentType);
    }
}
