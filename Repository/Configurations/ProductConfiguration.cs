using Entities.Domains;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repository.Configurations;
public class ProductConfiguration
{
    public ProductConfiguration(EntityTypeBuilder<Product> entityBuilder)
    {
        entityBuilder.HasKey(t => t.Id);
        entityBuilder.Property(t => t.Name).IsRequired();
    }
}
