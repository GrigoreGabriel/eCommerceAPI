using eCommerceAPI.Data.Addresses;
using eCommerceAPI.Data.Favorites;
using eCommerceAPI.Data.Orders;
using eCommerceAPI.Data.Products;
using eCommerceAPI.Data.UserPaymentTypes;

namespace eCommerceAPI.Data.Users
{
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public virtual Address? Address { get; set; }
        public ICollection<UserPaymentMethod>? UserPaymentMethods { get; set; }
        public virtual ICollection<Favorite>? Favorites { get; set; }
        public virtual ICollection<Order>? Orders { get; set; }

        public ICollection<Product>? Products { get; set; }

        public User() { }
        public User(Guid id)
        {
            Id = id;
        }

    }
}
