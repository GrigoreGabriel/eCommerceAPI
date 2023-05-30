using eCommerceAPI.Data.Favorites;
using eCommerceAPI.Data.ProductCategories;
using eCommerceAPI.Data.ProductItems;
using eCommerceAPI.Data.Users;

namespace eCommerceAPI.Data.Products
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int ProductCategoryId { get; set; }
        public virtual ProductCategory ProductCategory { get; set; }
        public virtual ICollection<ProductItem> ProductItems { get; set; }
        public virtual ICollection<Favorite>? Favorites { get; set; }

        public virtual ICollection<User>? Users { get; set; }
        public string Gender { get; set; }
        public string Size { get; set; }
        public string Image_Url { get; set; }
        public Product() { }

    }
}
