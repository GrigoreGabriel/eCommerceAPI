namespace eCommerceAPI.Business.Products.Queries.GetAll
{
    public class GetProductListResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ProductCategory { get; set; }
        public string Size { get; set; }
        public string Image_Url { get; set; }

        public int Price { get; set; }
    }
}
