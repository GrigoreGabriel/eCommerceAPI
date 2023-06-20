using eCommerceAPI.Data.ProductItems;

namespace eCommerceAPI.Data.Suppliers
{
    public class Supplier
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PersonOfContact { get; set; }
        public string PhoneNumber { get; set; }
        public string RegistrationNumber { get; set; }
        public ICollection<ProductItem> ProductItems { get; set; }

    }
}
