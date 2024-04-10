namespace ReviewManager.Core.Entities;

public class User : BaseEntity
{
    public string Name { get; set; }
    public string Email { get; set; }
    public IList<Review> Reviews { get; set; }
}
