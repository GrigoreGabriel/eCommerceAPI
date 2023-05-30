using eCommerceAPI.Data.Products;

namespace eCommerceAPI.Data.ProductItems
{
    public class ProductItem
    {
        public int Id { get; set; }
        public int QtyInStock { get; set; }
        public int Price { get; set; }
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
        public ProductItem() { }
    }
}
