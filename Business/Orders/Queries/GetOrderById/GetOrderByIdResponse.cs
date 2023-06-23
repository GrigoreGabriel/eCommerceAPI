namespace eCommerceAPI.Business.Orders.Queries.GetOrderById
{
    public class GetOrderByIdResponse
    {
        public Guid Id { get; set; }
        public DateTime OrderDate { get; set; }
        public string ProductName { get; set; }
        public string ImageUrl { get; set; }
        public string ProductType { get; set; }
        public int Quantity { get; set; }
        public int TotalValue { get; set; }
    }
}
