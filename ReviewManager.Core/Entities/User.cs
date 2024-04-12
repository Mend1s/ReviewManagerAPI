namespace ReviewManager.Core.Entities;

public class User : BaseEntity
{
    public User(string name, string email)
    {
        Name = name;
        Email = email;
    }
    public string Name { get; set; }
    public string Email { get; set; }
    public List<Review> Reviews { get; set; }

    public void UpdateUser(string name, string email)
    {
        Name = name;
        Email = email;
    }
}
