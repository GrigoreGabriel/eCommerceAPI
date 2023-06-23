namespace eCommerceAPI.Business.Orders.Queries.GetUsersOrders
{
    public class GetUsersOrdersResponse
    {
        public Guid Id { get; set; }
        public DateTime OrderDate { get; set; }
        public int TotalValue { get; set; }
    }
}
