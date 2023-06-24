namespace eCommerceAPI.Business.Orders.Queries.GetAllOrders
{
    public class GetAllOrdersResponse
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public DateTime OrderDate { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsShipped { get;set; }
        public int TotalValue { get; set; }
    }
}
