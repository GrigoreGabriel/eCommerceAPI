namespace eCommerceAPI.Business.Products.Queries.GetProductPrice
{
    public class GetProductPriceRequest
    {
        public int ProductId { get; set; }
        public string Type { get; set; }
        public string Size { get; set; }
    }
}
