using Microsoft.EntityFrameworkCore;
using ReviewManager.Core.Entities;
using ReviewManager.Core.Repositories;

namespace ReviewManager.Infrastructure.Persistence.Repositories;

public class ReviewRepository : IReviewRepository
{
    private readonly ReviewDbContext _dbContext;

    public ReviewRepository(ReviewDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Review> CreateReview(Review review)
    {
        await _dbContext.Reviews.AddAsync(review);
        await _dbContext.SaveChangesAsync();
        return review;
    }

    public async Task<IEnumerable<Review>> GetAllReviews()
    {
        return await _dbContext.Reviews.ToListAsync();
    }

    public async Task<Review> GetReviewById(int id)
    {
        return await _dbContext.Reviews.SingleOrDefaultAsync(r => r.Id == id);
    }
}
