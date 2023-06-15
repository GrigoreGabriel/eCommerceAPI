namespace eCommerceAPI.Business.Products.Queries.GetProductStock
{
    public class GetProductStockResponse
    {
        public string Brand { get; set; }
        public string Name { get; set; }
        public int QtyInStock { get; set; }
        public int Price { get; set; }
        public string Size { get; set; }
    }
}
