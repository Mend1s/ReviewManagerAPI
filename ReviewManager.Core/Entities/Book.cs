using ReviewManager.Core.Enums;

namespace ReviewManager.Core.Entities;

public class Book : BaseEntity
{
    // TODO: Add validation in ISBN property (repository?)
    public Book(
        string title,
        string description,
        string iSBN,
        string author,
        string publisher,
        //BookGenre genre,
        int yearOfPublication,
        int numberOfPages)
    {
        Title = title;
        Description = description;
        ISBN = iSBN;
        Author = author;
        Publisher = publisher;
        //Genre = genre;
        YearOfPublication = yearOfPublication;
        NumberOfPages = numberOfPages;
        CreateDate = DateTime.Now;
    }

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
    public byte[]? ImageUrl { get; set; }
    public string? AverageGrade { get; set; }
    public List<Review> Reviews { get; set; }

    public void UpdateBook(
        string title,
        string description,
        string iSBN,
        string author,
        string publisher,
        BookGenre genre,
        int yearOfPublication,
        int numberOfPages,
        string averageGrade)
    {
        Title = title;
        Description = description;
        ISBN = iSBN;
        Author = author;
        Publisher = publisher;
        Genre = genre;
        YearOfPublication = yearOfPublication;
        NumberOfPages = numberOfPages;
        AverageGrade = averageGrade;
    }
}
