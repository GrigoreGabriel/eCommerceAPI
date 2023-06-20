namespace eCommerceAPI.Business.Products.Queries.GetProductStock
{
    public class GetProductStockResponse
    {
        public string Brand { get; set; }
        public string Name { get; set; }
        public int QtyInStock { get; set; }
        public string Type { get; set; }
        public string Supplier { get; set; }
        public int PurchasePrice { get; set; }
        public int Price { get; set; }
        public string Size { get; set; }
    }
}
