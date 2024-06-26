using ReviewManager.Application.InputModels;
using ReviewManager.Application.ViewModels;
using ReviewManager.Core.Entities;

namespace ReviewManager.Application.Services.Interfaces;

public interface IReviewService
{
    Task<List<ReviewViewModel>> GetAllReviews();
    Task<ReviewViewModel> GetReviewById(int id);
    Task<Review> CreateReview(CreateReviewInputModel createReviewInputModel);
}
