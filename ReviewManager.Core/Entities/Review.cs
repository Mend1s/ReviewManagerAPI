namespace ReviewManager.Core.Entities;

public class Review : BaseEntity
{
    public int Note { get; set; }
    public string Description { get; set; }
    public int UserId { get; set; }
    public int BookId { get; set; }
    //public Book Book { get; set; }
    //public User User { get; set; }
    public DateTime CreateDate { get; set; }
    //public DateTime UpdateDate { get; set; }
}
