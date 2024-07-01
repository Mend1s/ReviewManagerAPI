using Microsoft.AspNetCore.Http;
using QuestPDF.Fluent;
using ReviewManager.Application.Services.Interfaces;
using ReviewManager.Core.Entities;
using ReviewManager.Infrastructure.Persistence.Services;

namespace ReviewManager.Application.Services.Implementations;

public class PdfService : IPdfService
{
    public IResult GerarRelatorioProdutos(IEnumerable<Book> books)
    {
        var book = new QuestPdfService(books);
        var pdf = book.GeneratePdf();
        book.GeneratePdfAndShow();
        //using var stream = new MemoryStream();
        //book.GeneratePdf(stream);
        //return stream.ToArray();
        return Results.File(pdf, "application/pdf", "relatorio_produtos.pdf");
    }
}
