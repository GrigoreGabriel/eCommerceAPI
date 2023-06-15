using eCommerceAPI.Data.ProductItems;

namespace eCommerceAPI.Data.ProductTypes
{
    public class ProductType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<ProductItem> ProductItems { get; set; }
    }
}
