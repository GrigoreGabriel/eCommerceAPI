namespace eCommerceAPI.Business.Products.Commands.AddProduct
{
    public class AddProductCommand
    {
        public string Name { get; set; }
        public string Brand { get; set; }
        public string Description { get; set; }

        public string Gender { get; set; }
        public string Category { get; set; }
        public string Image_Url { get; set; }
    }
}
