using Microsoft.AspNetCore.Mvc;
using ReviewManager.Application.Services.Interfaces;
using ReviewManager.Infrastructure.Persistence.Services;

namespace ReviewManager.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReportsController : ControllerBase
{
    private readonly IReportReview _reportReview;
    private readonly IGenerativeTestAI _generativeAIService;

    public ReportsController(IReportReview reportReview, IGenerativeTestAI generativeAIService)
    {
        _reportReview = reportReview;
        _generativeAIService = generativeAIService;
    }

    /// <summary>
    /// Gera um relatório em Excel das avalições de livros e o salva como "Reviews.xlsx".
    /// </summary>
    /// <returns>Retorna um resultado Ok indicando que o relatório foi criado com sucesso.</returns>
    [HttpGet("generate-report")]
    public async Task<IActionResult> GenerateReport()
    {
        await _reportReview.GenerateReportAsync();
        return Ok("Relatório criado com sucesso!");
    }

    [HttpGet("generate-book")]
    public async Task<IActionResult> Generate()
    {
        var response = await _generativeAIService.GenerateContentAsync();
        return Ok(response);
    }
}
