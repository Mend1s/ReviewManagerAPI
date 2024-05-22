using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReviewManager.Application.InputModels;
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

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CreateReviewInputModel createReviewInputModel)
    {
        var reviews = new Review(
            createReviewInputModel.Note,
            createReviewInputModel.Description,
            createReviewInputModel.IdUser,
            createReviewInputModel.IdBook);

        await _dbContext.Reviews.AddAsync(reviews);
        
        await _dbContext.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = reviews.Id }, createReviewInputModel);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Review>>> GetAll()
    {
        var reviews = await _dbContext.Reviews.ToListAsync();

        if (reviews is null) return BadRequest();

        return reviews;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Review>> GetById(int id)
    {
        var review = await _dbContext.Reviews.SingleOrDefaultAsync(r => r.Id == id);

        if (review is null) return NotFound();

        return review;
    }
}
