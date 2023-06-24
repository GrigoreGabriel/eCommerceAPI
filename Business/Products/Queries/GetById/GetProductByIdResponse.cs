namespace eCommerceAPI.Business.Products.Queries.GetById
{
    public class GetProductByIdResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ProductCategory { get; set; }
        public string Brand { get; set; }
        public string Image_Url { get; set; }

        public int Price { get; set; }
    }
}
