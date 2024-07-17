using Microsoft.AspNetCore.Mvc;
using ReviewManager.Application.Services.Interfaces;

namespace ReviewManager.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReportsController : ControllerBase
{
    private readonly IReportReview _reportReview;

    public ReportsController(IReportReview reportReview)
    {
        _reportReview = reportReview;
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
}
