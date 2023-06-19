namespace eCommerceAPI.Business.Carts.Queries
{
    public class GetCartContentsResponse
    {
        public int Id { get; set; }
        public string ProductImageUrl { get; set; }
        public string ProductName { get; set; }
        public string ProductItemType { get; set; }
        public int Quantity { get; set; }
        public int TotalItemValue { get; set; }


    }
}
