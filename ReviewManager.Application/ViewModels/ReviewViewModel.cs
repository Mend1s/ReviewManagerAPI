namespace ReviewManager.Application.ViewModels;

public class ReviewViewModel
{
    public int Id { get; set; }
    public int Note { get; set; }
    public string Description { get; set; }
    public int IdUser { get; set; }
    public int IdBook { get; set; }
    public DateTime CreateDate { get; set; }
}
