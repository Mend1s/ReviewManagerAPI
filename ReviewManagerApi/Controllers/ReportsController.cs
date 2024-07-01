using Microsoft.AspNetCore.Mvc;
using ReviewManager.Application.Services.Interfaces;
using ReviewManager.Core.Repositories;

namespace ReviewManager.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReportsController : ControllerBase
{
    private readonly IBookService _bookService;
    private readonly IBookRepository _bookRepository;
    private readonly IPdfService _pdfService;

    public ReportsController(IBookService bookService, IPdfService pdfService, IBookRepository bookRepository)
    {
        _bookService = bookService;
        _pdfService = pdfService;
        _bookRepository = bookRepository;
    }

    //[HttpGet("relatorio")]
    //public async Task<IResult> GerarRelatorio()
    //{
    //    var books = await _bookRepository.GetAllAsync();
    //    var pdfBytes = _pdfService.GerarRelatorioProdutos(books);
    //    return pdfBytes;
    //}
}
