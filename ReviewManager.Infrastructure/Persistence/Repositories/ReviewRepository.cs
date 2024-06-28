using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ReviewManager.Core.Entities;
using ReviewManager.Core.Repositories;

namespace ReviewManager.Infrastructure.Persistence.Repositories;

public class ReviewRepository : IReviewRepository
{
    private readonly ReviewDbContext _dbContext;
    private readonly ILogger<ReviewRepository> _logger; 

    public ReviewRepository(
        ReviewDbContext dbContext,
        ILogger<ReviewRepository> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<Review> CreateReview(Review review)
    {
        _logger.LogInformation("[ReviewRepository] Iniciando a criação de uma nova avaliação.");

        try
        {
            await _dbContext.Reviews.AddAsync(review);
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("[ReviewRepository] Avaliação criada com sucesso com ID: {Id}.", review.Id);
            return review;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar a avaliação.");
            throw;
        }
    }

    public async Task<IEnumerable<Review>> GetAllReviews()
    {
        _logger.LogInformation("[ReviewRepository] Iniciando a obtenção de todas as avaliações.");

        try
        {
            var reviews = await _dbContext.Reviews.ToListAsync();
            return reviews;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[ReviewRepository] Erro ao obter todas as avaliações.");
            throw;
        }
    }

    public async Task<Review> GetReviewById(int id)
    {
        _logger.LogInformation("[ReviewRepository] Iniciando a obtenção da avaliação com ID: {Id}.", id);

        try
        {
            var review = await _dbContext.Reviews.SingleOrDefaultAsync(r => r.Id == id);
            if (review == null)
            {
                _logger.LogWarning("[ReviewRepository] Avaliação com ID: {Id} não encontrada.", id);
            }
            else
            {
                _logger.LogInformation("[ReviewRepository] Avaliação com ID: {Id} obtida com sucesso.", id);
            }

            return review;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[ReviewRepository] Erro ao obter a avaliação com ID: {Id}.", id);
            throw;
        }
    }
}
