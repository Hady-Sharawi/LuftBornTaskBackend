namespace Entities.Domains;
public class User : BaseEntity
{
    // Leave empty constructor for EF Core
    public User()
    {

    }

    public User(string userName, string email)
    {
        UserName = userName;
        Email = email;
        CreatedDate = DateTime.UtcNow;
    }

    public string UserName { get; private set; }
    public string Email { get; private set; }
    public List<Product>? UserProducts { get; private set; }

    public void AddProduct(Product product)
    {
        if (product == null)
        {
            return;
        }

        //if (UserProducts.Any(p => p.Id == product.Id))
        //{
        //    throw new InvalidOperationException("This product is already added to the user's product list.");
        //}

        UserProducts.Add(product);
    }
}
