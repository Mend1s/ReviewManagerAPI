using OfficeOpenXml;
using ReviewManager.Application.InputModels;
using ReviewManager.Application.Services.Interfaces;
using ReviewManager.Core.Repositories;

namespace ReviewManager.Application.Services.Implementations;

public class ReportReviewService : IReportReview
{
    private readonly IReviewRepository _reviewRepository;

    public ReportReviewService(IReviewRepository reviewRepository)
    {
        _reviewRepository = reviewRepository;
    }

    public async Task GenerateReportAsync()
    {
        // Busca todas as reviews no banco de dados
        var reviews = await _reviewRepository.GetAllReviews();

        // Define a licença para uso não comercial
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        using (ExcelPackage excelPackage = new ExcelPackage())
        {
            string pathReport = @"C:\Users\André Mendes\source\repos\Mend1s\ReviewManagerAPI\ReviewManagerApi\Reports\";

            // Adiciona uma nova planilha
            ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("Reviews");

            // Adiciona o cabeçalho
            worksheet.Cells[1, 1].Value = "Id Review";
            worksheet.Cells[1, 2].Value = "Nota";
            worksheet.Cells[1, 3].Value = "Descrição";
            worksheet.Cells[1, 4].Value = "Id Usuário";
            worksheet.Cells[1, 5].Value = "Id Livro";
            worksheet.Cells[1, 6].Value = "Data da Avaliação";

            // Centraliza o cabeçalho
            worksheet.Cells["A1:F1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

            // Adiciona as reviews
            int rowIndex = 2;
            foreach (var review in reviews)
            {
                worksheet.Cells[rowIndex, 1].Value = review.Id;
                worksheet.Cells[rowIndex, 2].Value = review.Note;
                worksheet.Cells[rowIndex, 3].Value = review.Description;
                worksheet.Cells[rowIndex, 4].Value = review.IdUser;
                worksheet.Cells[rowIndex, 5].Value = review.IdBook;
                worksheet.Cells[rowIndex, 6].Value = review.CreateDate.Date;

                worksheet.Cells[rowIndex, 6].Style.Numberformat.Format = "dd/MM/yyyy";

                rowIndex++;
            }

            // Auto ajusta o tamanho das colunas
            worksheet.Column(1).AutoFit();
            worksheet.Column(2).AutoFit();
            worksheet.Column(3).AutoFit();
            worksheet.Column(4).AutoFit();
            worksheet.Column(5).AutoFit();
            worksheet.Column(6).AutoFit();

            // Salva o arquivo
            var filePath = Path.Combine(pathReport, "Reviews.xlsx");
            File.WriteAllBytes(filePath, excelPackage.GetAsByteArray());
        }
    }
}
