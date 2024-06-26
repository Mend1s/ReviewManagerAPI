using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReviewManager.Application.InputModels;
using ReviewManager.Application.ViewModels;
using ReviewManager.Core.Entities;
using ReviewManager.Infrastructure.Persistence;

namespace ReviewManager.API.Controllers;

[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    private readonly ReviewDbContext _dbContext;
    public BooksController(ReviewDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<BookViewModel>>> GetAll()
    {
        var books = await _dbContext.Books.ToListAsync();

        if (books is null) return BadRequest();

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
            AverageGrade = book.AverageGrade
        }).ToList();

        return bookViewModel;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<BookViewModel>> GetById(int id)
    {
        var book = await _dbContext.Books.SingleOrDefaultAsync(b => b.Id == id);

        if (book is null) return NotFound();

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
            //ImageUrl = book.ImageUrl,
            AverageGrade = book.AverageGrade
        };

        return bookViewModel;
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CreateBookInputModel createBookInputModel)
    {
        var book = new Book(
            createBookInputModel.Title,
            createBookInputModel.Description,
            createBookInputModel.ISBN,
            createBookInputModel.Author,
            createBookInputModel.Publisher,
            createBookInputModel.Genre,
            createBookInputModel.YearOfPublication,
            createBookInputModel.NumberOfPages);

        await _dbContext.Books.AddAsync(book);

        await _dbContext.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = book.Id }, createBookInputModel);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody] UpdateBookInputModel updateBookModel)
    {
        var book = _dbContext.Books.SingleOrDefault(b => b.Id == id);

        if (book is null) return BadRequest();

        book.UpdateBook(
            updateBookModel.Title,
            updateBookModel.Description,
            updateBookModel.ISBN,
            updateBookModel.Author,
            updateBookModel.Publisher,
            updateBookModel.Genre,
            updateBookModel.YearOfPublication,
            updateBookModel.NumberOfPages,
            updateBookModel.AverageGrade);

        _dbContext.Update(book);

        await _dbContext.SaveChangesAsync();

        return Ok(book);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var book = _dbContext.Books.SingleOrDefault(b => b.Id == id);

        if (book is null) return NotFound();

        _dbContext.Books.Remove(book);

        await _dbContext.SaveChangesAsync();

        return Ok(book);
    }
}
