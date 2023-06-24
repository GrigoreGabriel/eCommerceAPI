using eCommerceAPI.Data.Addresses;
using eCommerceAPI.Data.OrderDetails;
using eCommerceAPI.Data.Users;

namespace eCommerceAPI.Data.Orders
{
    public class Order
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public int AddressId { get; set; }
        public virtual User User { get; set; }
        public virtual Address Address { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public int OrderTotal { get; set; }
        public bool IsShipped { get; set; }
        public DateTime OrderTime { get; set; }
    }
}
