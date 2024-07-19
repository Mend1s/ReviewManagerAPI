using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ReviewManager.Application.InputModels;
using ReviewManager.Application.Services.Interfaces;
using ReviewManager.Application.ViewModels;
using ReviewManager.Core.Entities;
using ReviewManager.Core.Repositories;

namespace ReviewManager.Application.Services.Implementations;

public class BookService : IBookService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<BookService> _logger;
    private readonly IBookRepository _bookRepository;
    public BookService(
        ILogger<BookService> logger,
        IBookRepository bookRepository,
        IHttpContextAccessor httpContextAccessor)
    {
        _logger = logger;
        _bookRepository = bookRepository;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<Book> CreateBook(CreateBookInputModel createBookInputModel)
    {
        _logger.LogInformation("Entrando no CreateBook do BookService para criar um novo livro.");

        var fileName = Path.GetFileName(createBookInputModel.ImageUrl.FileName);
        var filePath = Path.Combine("wwwroot", "Storage", fileName);

        using Stream fileStream = new FileStream(filePath, FileMode.Create);
        createBookInputModel.ImageUrl.CopyTo(fileStream);

        var relativeFilePath = Path.Combine("Storage", fileName);

        var book = new Book(
            createBookInputModel.Title,
            createBookInputModel.Description,
            createBookInputModel.ISBN,
            createBookInputModel.Author,
            createBookInputModel.Publisher,
            createBookInputModel.Genre,
            createBookInputModel.YearOfPublication,
            createBookInputModel.NumberOfPages,
            relativeFilePath);

        var bookCreated = await _bookRepository.CreateAsync(book);

        return bookCreated;
    }

    public async Task<bool> DeleteBook(int id)
    {
        _logger.LogInformation("Entrando no DeleteBook do BookService para excluir um livro.");

        var book = await _bookRepository.GetByIdAsync(id);

        if (book is null) throw new Exception("Livro não encontrado.");

        await _bookRepository.DeleteAsync(book);

        return true;
    }

    public async Task<List<BookViewModel>> GetAllBooks()
    {
        _logger.LogInformation("Entrando no GetAllBooks do BookService para retornar uma lista de livros.");

        var books = await _bookRepository.GetAllAsync();

        if (books is null) throw new Exception("Livros não encontrado.");

        var request = _httpContextAccessor.HttpContext.Request;
        var baseUrl = $"{request.Scheme}://{request.Host}/";

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
            ImageUrl = $"{baseUrl}{book.ImageUrl.Replace("\\", "/")}"
        }).ToList();

        return bookViewModel;
    }

    public async Task<BookViewModel> GetBookById(int id)
    {
        _logger.LogInformation("Entrando no GetBookById do BookService para retornar um livro.");

        var book = await _bookRepository.GetByIdAsync(id);

        if (book is null) throw new Exception("Livro não encontrado.");

        var request = _httpContextAccessor.HttpContext.Request;
        var baseUrl = $"{request.Scheme}://{request.Host}/";

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
            ImageUrl = $"{baseUrl}{book.ImageUrl.Replace("\\", "/")}",
            AverageGrade = book.AverageGrade
        };

        return bookViewModel;
    }

    public async Task<Book> UpdateBook(int id, UpdateBookInputModel updateBookInputModel)
    {
        _logger.LogInformation("Entrando no UpdateBook do BookService para atualizar um livro.");

        var book = await _bookRepository.GetByIdAsync(id);

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

        await _bookRepository.UpdateAsync(book);

        return book;
    }

    public async Task<FileResult> DownloadPhotoBookAsync(int id)
    {
        _logger.LogInformation("Entrando no DownloadPhotoBookAsync do BookService para baixar a imagem de capa do livro.");

        // Busca o livro no banco
        var book = await _bookRepository.GetByIdAsync(id);

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
