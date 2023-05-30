using eCommerceAPI.Data.Users;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using eCommerceAPI.Data.UserPaymentTypes;

namespace eCommerceAPI.Data.UserPaymentMethods.Configurations
{
    public class UserPaymentMethodConfiguration : IEntityTypeConfiguration<UserPaymentMethod>
    {
        public void Configure(EntityTypeBuilder<UserPaymentMethod> builder)
        {
            builder.ToTable(name: nameof(UserPaymentMethod));
            builder.HasKey(x => x.Id);
        }
    }
}
