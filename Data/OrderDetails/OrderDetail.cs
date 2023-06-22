using eCommerceAPI.Data.Orders;
using eCommerceAPI.Data.ProductItems;

namespace eCommerceAPI.Data.OrderDetails
{
    public class OrderDetail
    {
        public int Id { get; set; }
        public Guid OrderId { get; set; }
        public int ProductItemId { get; set; }
        public int Quantity { get; set; }
        public int ItemsTotalValue { get; set; }
        public virtual ProductItem ProductItem { get; set; }
        public virtual Order Order { get; set; }


    }
}
