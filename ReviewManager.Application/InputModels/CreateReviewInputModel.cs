namespace ReviewManager.Application.InputModels;

public class CreateReviewInputModel
{
    public int Note { get; set; }
    public string Description { get; set; }
    public int IdUser { get; set; }
    public int IdBook { get; set; }
}
