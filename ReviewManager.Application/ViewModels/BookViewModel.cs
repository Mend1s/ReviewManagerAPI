using ReviewManager.Core.Entities;
using ReviewManager.Core.Enums;

namespace ReviewManager.Application.ViewModels;

public class BookViewModel
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string ISBN { get; set; }
    public string Author { get; set; }
    public string Publisher { get; set; }
    public BookGenre Genre { get; set; }
    public int YearOfPublication { get; set; }
    public int NumberOfPages { get; set; }
    public DateTime CreateDate { get; set; }
    public string ImageUrl { get; set; }
    public decimal? AverageGrade { get; set; }
    //public List<Review> Reviews { get; set; }
}
