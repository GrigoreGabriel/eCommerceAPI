namespace eCommerceAPI.Business.Carts.Commands
{
    public class AddProductToCartRequest
    {
        public Guid UserId { get; set; }
        public int ProductItemId { get; set; }
        public int Quantity { get; set; }
    }
}
