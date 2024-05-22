namespace ReviewManager.Core.Entities;

public class Review : BaseEntity
{
    public Review
        (int note,
        string description,
        int idUser,
        int idBook)
    {
        Note = note;
        Description = description;
        IdUser = idUser;
        IdBook = idBook;
        CreateDate = DateTime.Now;
    }

    public int Note { get; set; }
    public string Description { get; set; }
    public int IdUser { get; set; }
    public int IdBook { get; set; }
    public Book Book { get; set; }
    public User User { get; set; }
    public DateTime CreateDate { get; set; }
}
