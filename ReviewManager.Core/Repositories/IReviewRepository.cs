using ReviewManager.Core.Entities;

namespace ReviewManager.Core.Repositories;

public interface IReviewRepository
{
    Task<Review> CreateReview(Review review);
    Task<IEnumerable<Review>> GetAllReviews();
    Task<Review> GetReviewById(int id);
}
