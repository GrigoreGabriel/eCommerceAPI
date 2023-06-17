using eCommerceAPI.Data.ShoppingCartItems;
using eCommerceAPI.Data.Users;

namespace eCommerceAPI.Data.ShoppingCarts
{
    public class ShoppingCart
    {
        public int Id { get; set; }
        public Guid? UserId { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<ShoppingCartItem> ShoppingCartItems { get; set; }
    }
}
