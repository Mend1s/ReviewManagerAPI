using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReviewManager.Application.InputModels;
using ReviewManager.Application.Services.Implementations;
using ReviewManager.Application.Services.Interfaces;
using ReviewManager.Application.ViewModels;
using ReviewManager.Core.Entities;
using ReviewManager.Infrastructure.Persistence;

namespace ReviewManager.API.Controllers;

[Route("api/[controller]")]
public class ReviewsController : ControllerBase
{
    private readonly IReviewService _reviewService;
    public ReviewsController(IReviewService reviewService)
    {
        _reviewService = reviewService;
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
        try
        {
            return Ok(await _reviewService.GetAllReviews());
        }
        catch (Exception ex)
        {
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
        try
        {
            return Ok(await _reviewService.GetReviewById(id));
        }
        catch (Exception ex)
        {
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
        try
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var user = await _reviewService.CreateReview(reviewViewModel);

            return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
        }
        catch (Exception ex)
        {
            return BadRequest(error: $"[CreateReview] : {ex.Message}");
        }
    }
}
