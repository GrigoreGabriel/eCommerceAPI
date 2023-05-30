using eCommerceAPI.Data.Products;
using eCommerceAPI.Data.Users;

namespace eCommerceAPI.Data.Favorites
{
    public class Favorite
    {
        public Guid UserId { get; set; }
        public virtual User User { get; set; }
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
    }
}
