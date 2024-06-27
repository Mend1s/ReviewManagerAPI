using Microsoft.EntityFrameworkCore;
using ReviewManager.Application.InputModels;
using ReviewManager.Application.Services.Interfaces;
using ReviewManager.Application.ViewModels;
using ReviewManager.Core.Entities;
using ReviewManager.Infrastructure.Persistence;

namespace ReviewManager.Application.Services.Implementations;

public class ReviewService : IReviewService
{
    private readonly ReviewDbContext _dbContext;
    public ReviewService(ReviewDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Review> CreateReview(CreateReviewInputModel createReviewInputModel)
    {
        if (createReviewInputModel.Note < 1 || createReviewInputModel.Note > 5)
        {
            throw new ArgumentException("Nota deve ser entre 1 e 5.");
        }

        var review = new Review(
            createReviewInputModel.Note,
            createReviewInputModel.Description,
            createReviewInputModel.IdUser,
            createReviewInputModel.IdBook);

        var user = await _dbContext.Users.SingleOrDefaultAsync(u => u.Id == createReviewInputModel.IdUser);
        if (user is null) throw new Exception("Usuário não encontrado, por favor tente novamente com os dados corretos.");

        var book = await _dbContext.Books.SingleOrDefaultAsync(b => b.Id == createReviewInputModel.IdBook);
        if (book is null) throw new Exception("Livro não encontrado, por favor tente novamente com os dados corretos.");

        //review.User = user;
        //review.Book = book;

        _dbContext.Reviews.Add(review);
        await _dbContext.SaveChangesAsync();

        //var totalReviews = book.Reviews.Count;
        //var sumOfNotes = book.Reviews.Sum(a => a.Note);
        //book.AverageGrade = (decimal)sumOfNotes / totalReviews;

        return review;
    }

    public async Task<List<ReviewViewModel>> GetAllReviews()
    {
        var reviews = await _dbContext.Reviews.ToListAsync();

        if (reviews is null) throw new Exception("Avaliações não encontradas.");

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

    public async Task<ReviewViewModel> GetReviewById(int id)
    {
        var review = await _dbContext.Reviews.SingleOrDefaultAsync(r => r.Id == id);

        if (review is null) throw new Exception("Avaliação não encontrada.");

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
}
