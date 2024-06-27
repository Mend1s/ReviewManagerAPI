using Microsoft.EntityFrameworkCore;
using ReviewManager.Application.InputModels;
using ReviewManager.Application.Services.Interfaces;
using ReviewManager.Application.ViewModels;
using ReviewManager.Core.Entities;
using ReviewManager.Core.Repositories;
using ReviewManager.Infrastructure.Persistence;

namespace ReviewManager.Application.Services.Implementations;

public class ReviewService : IReviewService
{
    private readonly IReviewRepository _reviewRepository;
    private readonly IUserRepository _userRepository;
    private readonly IBookRepository _bookRepository;
    public ReviewService(
        IReviewRepository reviewRepository,
        IUserRepository userRepository,
        IBookRepository bookRepository)
    {
        _reviewRepository = reviewRepository;
        _userRepository = userRepository;
        _bookRepository = bookRepository;
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

        var user = await _userRepository.GetByIdAsync(createReviewInputModel.IdUser);
        if (user is null) throw new Exception("Usuário não encontrado, por favor tente novamente com os dados corretos.");

        var book = await _bookRepository.GetByIdAsync(createReviewInputModel.IdBook);
        if (book is null) throw new Exception("Livro não encontrado, por favor tente novamente com os dados corretos.");

        var reviewCreated = await _reviewRepository.CreateReview(review);

        //var totalReviews = book.Reviews.Count;
        //var sumOfNotes = book.Reviews.Sum(a => a.Note);
        //book.AverageGrade = (decimal)sumOfNotes / totalReviews;

        return reviewCreated;
    }

    public async Task<List<ReviewViewModel>> GetAllReviews()
    {
        var reviews = await _reviewRepository.GetAllReviews();

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
        var review = await _reviewRepository.GetReviewById(id);

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
