using eCommerceAPI.Data.Addresses;
using eCommerceAPI.Data.Favorites;
using eCommerceAPI.Data.OrderDetails;
using eCommerceAPI.Data.Orders;
using eCommerceAPI.Data.PaymentTypes;
using eCommerceAPI.Data.ProductCategories;
using eCommerceAPI.Data.ProductItems;
using eCommerceAPI.Data.Products;
using eCommerceAPI.Data.ProductTypes;
using eCommerceAPI.Data.ShoppingCartItems;
using eCommerceAPI.Data.ShoppingCarts;
using eCommerceAPI.Data.Suppliers;
using eCommerceAPI.Data.UserPaymentTypes;
using eCommerceAPI.Data.Users;
using Microsoft.EntityFrameworkCore;

namespace eCommerceAPI.Data
{
    public class CommerceDbContext : DbContext
    {
        public CommerceDbContext(DbContextOptions<CommerceDbContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<PaymentType> PaymentTypes { get; set; }
        public DbSet<UserPaymentMethod> UserPaymentMethods { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductItem> ProductItems { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<ShoppingCartItem> ShoppingCartItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<ProductType> ProductTypes { get; set; }
        public DbSet<Favorite> Favorites { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Favorite>()
                .HasKey(f => new { f.UserId, f.ProductId });

            modelBuilder.Entity<Favorite>()
                .HasOne(f => f.User)
                .WithMany(u => u.Favorites)
                .HasForeignKey(f => f.UserId);

            modelBuilder.Entity<Favorite>()
                .HasOne(f => f.Product)
                .WithMany(p => p.Favorites)
                .HasForeignKey(f => f.ProductId);

            modelBuilder.Entity<Favorite>()
                .ToTable("Favorites");
            modelBuilder.Entity<ProductType>().HasData
                (
                new ProductType { Id = 1, Name = "Perfume" },
                new ProductType { Id = 2, Name = "Eau de perfume" },
                new ProductType { Id = 3, Name = "Eau de toilette" }
                );
        }
    }
}
