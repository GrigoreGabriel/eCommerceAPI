namespace eCommerceAPI.Business.Orders.Queries.GetOrderByUserId
{
    public class GetOrderByUserIdResponse
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string AdressName { get; set; }
        public string AdressPostalCode { get; set; }
        public int OrderTotal { get; set; }
    }
}
