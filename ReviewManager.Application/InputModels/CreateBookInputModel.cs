using ReviewManager.Core.Enums;

namespace ReviewManager.Application.InputModels;

public class CreateBookInputModel
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string ISBN { get; set; }
    public string Author { get; set; }
    public string Publisher { get; set; }
    public BookGenre Genre { get; set; }
    public int YearOfPublication { get; set; }
    public int NumberOfPages { get; set; }
    public DateTime CreateDate { get; set; }
    //public string Format { get; set; }
    //public byte[]? ImageUrl { get; set; }
    //public string AverageGrade { get; set; }
}
