using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eCommerceAPI.Data.Users.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable(name: nameof(User));
            builder.HasKey(x => x.Id);
            builder.HasMany(x => x.UserPaymentMethods).WithOne(x => x.User).OnDelete(DeleteBehavior.NoAction);
        }
    }
}
