using ReviewManager.Core.Enums;

namespace ReviewManager.Core.Entities;

public class Book : BaseEntity
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
    public string Format { get; set; }
    public string ImageUrl { get; set; }
    public string AverageGrade { get; set; }
    public ICollection<Review> Reviews { get; set; }
}
