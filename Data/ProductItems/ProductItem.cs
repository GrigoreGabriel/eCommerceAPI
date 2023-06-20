using eCommerceAPI.Data.Products;
using eCommerceAPI.Data.ProductTypes;
using eCommerceAPI.Data.Suppliers;

namespace eCommerceAPI.Data.ProductItems
{
    public class ProductItem
    {
        public int Id { get; set; }
        public int QtyInStock { get; set; }
        public int Price { get; set; }
        public int PurchasePrice { get; set; }
        public string Size { get; set; }
        public int ProductId { get; set; }
        public int ProductTypeId { get; set; }
        public int SupplierId { get; set; }
        public virtual Product Product { get; set; }
        public virtual ProductType ProductType { get; set; }
        public virtual Supplier Supplier { get; set; }
        public ProductItem() { }
    }
}
