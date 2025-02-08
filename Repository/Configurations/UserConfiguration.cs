using Entities.Domains;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repository.Configurations;
public class UserConfiguration
{
    public UserConfiguration(EntityTypeBuilder<User> entityBuilder)
    {
        entityBuilder.HasKey(x => x.Id);
        entityBuilder.Property(x => x.UserName).IsRequired();
        entityBuilder.Property(x => x.Email).IsRequired();

        #region RelashionShips
        entityBuilder
            .HasMany(x => x.UserProducts)
            .WithOne(u => u.User)
            .HasForeignKey(x => x.UserId);
        #endregion
    }
}
