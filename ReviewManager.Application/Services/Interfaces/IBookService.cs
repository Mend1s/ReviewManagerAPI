using Microsoft.AspNetCore.Mvc;
using ReviewManager.Application.InputModels;
using ReviewManager.Application.ViewModels;
using ReviewManager.Core.Entities;

namespace ReviewManager.Application.Services.Interfaces;

public interface IBookService
{
    Task<List<BookViewModel>> GetAllBooks();
    Task<BookViewModel> GetBookById(int id);
    Task<Book> CreateBook(CreateBookInputModel createBookInputModel);
    Task<Book> UpdateBook(int id, UpdateBookInputModel updateBookInputModel);
    Task<bool> DeleteBook(int id);
    Task<FileResult> DownloadPhotoBookAsync(int id);
}
