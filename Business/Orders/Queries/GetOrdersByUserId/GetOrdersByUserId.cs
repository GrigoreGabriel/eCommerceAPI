namespace eCommerceAPI.Business.Orders.Queries.GetOrdersByUserId
{
    public class GetOrdersByUserId
    {
        public Guid Id { get; set; }
        public DateTime OrderDate { get; set; }
        public bool IsShipped { get; set; }
        public int TotalValue { get; set; }
    }
}
