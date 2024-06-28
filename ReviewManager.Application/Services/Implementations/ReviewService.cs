using Microsoft.Extensions.Logging;
using ReviewManager.Application.InputModels;
using ReviewManager.Application.Services.Interfaces;
using ReviewManager.Application.ViewModels;
using ReviewManager.Core.Entities;
using ReviewManager.Core.Repositories;

namespace ReviewManager.Application.Services.Implementations;

public class ReviewService : IReviewService
{
    private readonly IReviewRepository _reviewRepository;
    private readonly IUserRepository _userRepository;
    private readonly IBookRepository _bookRepository;
    private readonly ILogger<ReviewService> _logger; 

    public ReviewService(
        IReviewRepository reviewRepository,
        IUserRepository userRepository,
        IBookRepository bookRepository,
        ILogger<ReviewService> logger)
    {
        _reviewRepository = reviewRepository;
        _userRepository = userRepository;
        _bookRepository = bookRepository;
        _logger = logger; 
    }

    public async Task<Review> CreateReview(CreateReviewInputModel createReviewInputModel)
    {
        _logger.LogInformation("[ReviewService] Iniciando a criação de uma nova avaliação.");

        if (createReviewInputModel.Note < 1 || createReviewInputModel.Note > 5)
        {
            _logger.LogWarning("Nota inválida: {Note}. A nota deve ser entre 1 e 5.", createReviewInputModel.Note);
            throw new ArgumentException("Nota deve ser entre 1 e 5.");
        }

        var review = new Review(
            createReviewInputModel.Note,
            createReviewInputModel.Description,
            createReviewInputModel.IdUser,
            createReviewInputModel.IdBook);

        _logger.LogInformation("Verificando a existência do usuário com ID: {IdUser}.", createReviewInputModel.IdUser);
        var user = await _userRepository.GetByIdAsync(createReviewInputModel.IdUser);
        if (user is null)
        {
            _logger.LogWarning("Usuário com ID: {IdUser} não encontrado.", createReviewInputModel.IdUser);
            throw new Exception("Usuário não encontrado, por favor tente novamente com os dados corretos.");
        }

        _logger.LogInformation("Verificando a existência do livro com ID: {IdBook}.", createReviewInputModel.IdBook);
        var book = await _bookRepository.GetByIdAsync(createReviewInputModel.IdBook);
        if (book is null)
        {
            _logger.LogWarning("Livro com ID: {IdBook} não encontrado.", createReviewInputModel.IdBook);
            throw new Exception("Livro não encontrado, por favor tente novamente com os dados corretos.");
        }

        _logger.LogInformation("Criando a avaliação para o usuário com ID: {IdUser} e o livro com ID: {IdBook}.", createReviewInputModel.IdUser, createReviewInputModel.IdBook);
        var reviewCreated = await _reviewRepository.CreateReview(review);

        _logger.LogInformation("Avaliação criada com sucesso com ID: {Id}.", reviewCreated.Id);
        return reviewCreated;
    }

    public async Task<List<ReviewViewModel>> GetAllReviews()
    {
        _logger.LogInformation("[ReviewService] Iniciando a obtenção de todas as avaliações.");

        var reviews = await _reviewRepository.GetAllReviews();

        if (reviews is null)
        {
            _logger.LogWarning("Nenhuma avaliação encontrada.");
            throw new Exception("Avaliações não encontradas.");
        }

        var reviewViewModel = reviews.Select(review => new ReviewViewModel
        {
            Id = review.Id,
            Note = review.Note,
            Description = review.Description,
            IdUser = review.IdUser,
            IdBook = review.IdBook,
            CreateDate = review.CreateDate
        }).ToList();

        _logger.LogInformation("Obtenção de avaliações concluída com sucesso.");
        return reviewViewModel;
    }

    public async Task<ReviewViewModel> GetReviewById(int id)
    {
        _logger.LogInformation("[ReviewService] Iniciando a obtenção da avaliação com ID: {Id}.", id);

        var review = await _reviewRepository.GetReviewById(id);

        if (review is null)
        {
            _logger.LogWarning("Avaliação com ID: {Id} não encontrada.", id);
            throw new Exception("Avaliação não encontrada.");
        }

        var reviewViewModel = new ReviewViewModel
        {
            Id = review.Id,
            Note = review.Note,
            Description = review.Description,
            IdUser = review.IdUser,
            IdBook = review.IdBook,
            CreateDate = review.CreateDate
        };

        _logger.LogInformation("Avaliação com ID: {Id} obtida com sucesso.", id);
        return reviewViewModel;
    }
}
