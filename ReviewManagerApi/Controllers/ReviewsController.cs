using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReviewManager.Application.InputModels;
using ReviewManager.Application.ViewModels;
using ReviewManager.Core.Entities;
using ReviewManager.Infrastructure.Persistence;

namespace ReviewManager.API.Controllers;

[Route("api/[controller]")]
public class ReviewsController : ControllerBase
{
    private readonly ReviewDbContext _dbContext;
    public ReviewsController(ReviewDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ReviewViewModel>>> GetAll()
    {
        var reviews = await _dbContext.Reviews.ToListAsync();

        if (reviews is null) return BadRequest();

        var reviewViewModel = reviews.Select(review => new ReviewViewModel
        {
            Id = review.Id,
            Note = review.Note,
            Description = review.Description,
            IdUser = review.IdUser,
            IdBook = review.IdBook,
            CreateDate = review.CreateDate
        }).ToList();

        return reviewViewModel;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ReviewViewModel>> GetById(int id)
    {
        var review = await _dbContext.Reviews.SingleOrDefaultAsync(r => r.Id == id);

        if (review is null) return NotFound();

        var reviewViewModel = new ReviewViewModel
        {
            Id = review.Id,
            Note = review.Note,
            Description = review.Description,
            IdUser = review.IdUser,
            IdBook = review.IdBook,
            CreateDate = review.CreateDate
        };

        return reviewViewModel;
    }

    [HttpPost]
    public async Task<ActionResult<ReviewViewModel>> Create([FromBody] CreateReviewInputModel reviewViewModel)
    {
        var review = new Review(reviewViewModel.Note, reviewViewModel.Description, reviewViewModel.IdUser, idBook: reviewViewModel.IdBook);

        var user = await _dbContext.Users.SingleOrDefaultAsync(u => u.Id == reviewViewModel.IdUser);
        if (user is null) return NotFound("Usuário não encontrado, por favor tente novamente com os dados corretos.");

        var book = await _dbContext.Books.SingleOrDefaultAsync(b => b.Id == reviewViewModel.IdBook);
        if (book is null) return NotFound("Livro não encontrado, por favor tente novamente com os dados corretos.");

        _dbContext.Reviews.Add(review);
        await _dbContext.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = review.Id }, review);
    }
}
