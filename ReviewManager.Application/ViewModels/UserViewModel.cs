namespace ReviewManager.Application.ViewModels;

public class UserViewModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public IList<ReviewViewModel> Reviews { get; set; }
}
