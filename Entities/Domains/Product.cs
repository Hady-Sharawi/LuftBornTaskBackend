namespace Entities.Domains;
public class Product : BaseEntity
{
    // Leave empty constructor for EF Core
    public Product()
    {
        
    }
    public Product(string name)
    {
        Name = name;
        CreatedDate = DateTime.UtcNow;
    }

    public string Name { get; private set; }

    public int? UserId { get; private set; }
    public User User { get; private set; }

}
