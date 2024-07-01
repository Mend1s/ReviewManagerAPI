using Microsoft.AspNetCore.Mvc;
using ReviewManager.Application.InputModels;
using ReviewManager.Application.Services.Interfaces;
using ReviewManager.Application.ViewModels;

namespace ReviewManager.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReviewsController : ControllerBase
{
    private readonly IReviewService _reviewService;
    private readonly ILogger<ReviewsController> _logger;

    public ReviewsController(
        IReviewService reviewService,
        ILogger<ReviewsController> logger)
    {
        _reviewService = reviewService;
        _logger = logger;
    }

    /// <summary>
    /// Obtém todos as avaliações.
    /// </summary>
    /// <returns>Uma lista de avaliações.</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<ReviewViewModel>>> GetAll()
    {
        _logger.LogInformation("[ReviewsController] Iniciando a operação de obtenção de todas as avaliações.");

        try
        {
            var reviews = await _reviewService.GetAllReviews();

            if (reviews == null || !reviews.Any())
            {
                _logger.LogInformation("Nenhuma avaliação encontrada.");
                return NotFound();
            }

            _logger.LogInformation("Avaliações obtidas com sucesso.");
            return Ok(reviews);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter todas as avaliações.");
            return BadRequest(error: $"[GetAllReviews] : {ex.Message}");
        }
    }

    /// <summary>
    /// Obtém um avaliação pela ID.
    /// </summary>
    /// <param name="id">O ID da avaliação a ser obtido.</param>
    /// <returns>A avaliação solicitada.</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ReviewViewModel>> GetById(int id)
    {
        _logger.LogInformation("[ReviewsController] Iniciando a operação de obtenção da avaliação com ID: {Id}", id);

        try
        {
            var review = await _reviewService.GetReviewById(id);

            if (review == null)
            {
                _logger.LogInformation("Avaliação com ID: {Id} não encontrada.", id);
                return NotFound();
            }

            _logger.LogInformation("Avaliação com ID: {Id} obtida com sucesso.", id);
            return Ok(review);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter a avaliação com ID: {Id}.", id);
            return BadRequest(error: $"[GetReviewById] : {ex.Message}");
        }
    }

    /// <summary>
    /// Cria uma nova avaliação.
    /// </summary>
    /// <param name="reviewViewModel">Os detalhes da avaliação a ser criada.</param>
    /// <returns>A avaliação criada.</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ReviewViewModel>> CreateReview([FromBody] CreateReviewInputModel reviewViewModel)
    {
        _logger.LogInformation("[ReviewsController] Iniciando a operação de criação de uma nova avaliação.");

        try
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Modelo de entrada inválido.");
                return BadRequest(ModelState);
            }

            var review = await _reviewService.CreateReview(reviewViewModel);

            _logger.LogInformation("Avaliação criada com sucesso com ID: {Id}.", review.Id);
            return CreatedAtAction(nameof(GetById), new { id = review.Id }, review);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar uma nova avaliação.");
            return BadRequest(error: $"[CreateReview] : {ex.Message}");
        }
    }
}
