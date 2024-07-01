using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using ReviewManager.Core.Entities;

namespace ReviewManager.Infrastructure.Persistence.Services;

public class QuestPdfService : IDocument
{
    public IEnumerable<Book> Books { get;}

    public QuestPdfService(IEnumerable<Book> books)
    {
        Books = books;
    }

    public void Compose(IDocumentContainer container)
    {
        container
            .Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(2, Unit.Centimetre);
                page.PageColor(Colors.White);
                page.DefaultTextStyle(x => x.FontSize(12));

                page.Header()
                    .Text("Relatório de Produtos")
                    .SemiBold().FontSize(24).FontColor(Colors.Blue.Medium);

                page.Content()
                    .Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.ConstantColumn(50);
                            columns.RelativeColumn(3);
                            columns.RelativeColumn(1);
                            columns.RelativeColumn(1);
                        });

                        table.Header(header =>
                        {
                            header.Cell().Element(CellStyle).Text("Id");
                            header.Cell().Element(CellStyle).Text("Título");
                            header.Cell().Element(CellStyle).Text("Qtd. Avaliações");
                            header.Cell().Element(CellStyle).Text("N. de Páginas");
                        });

                        foreach (var book in Books)
                        {
                            table.Cell().Element(CellStyle).Text(book.Id.ToString());
                            table.Cell().Element(CellStyle).Text(book.Title);
                            table.Cell().Element(CellStyle).Text(book.Reviews.Count().ToString());
                            table.Cell().Element(CellStyle).Text(book.NumberOfPages.ToString());
                        }
                    });

                page.Footer()
                    .AlignCenter()
                    .Text(text =>
                    {
                        text.Span("Página ");
                        text.CurrentPageNumber();
                        text.Span(" de ");
                        text.TotalPages();
                    });
            });
    }

    private IContainer CellStyle(IContainer container)
    {
        return container
            .Border(1)
            .BorderColor(Colors.Black)
            .Padding(5);
    }
}
