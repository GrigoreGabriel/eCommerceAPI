using eCommerceAPI.Data.ProductItems;

namespace eCommerceAPI.Data.ShoppingCartItems
{
    public class ShoppingCartItem
    {
        public int Id { get; set; }
        public int ProductItemId { get; set; }
        public virtual ProductItem ProductItem { get; set; }
        public int ShoppingCartId { get; set; }
        public int Quantity { get; set; }
        public ShoppingCartItem() { }
    }
}
