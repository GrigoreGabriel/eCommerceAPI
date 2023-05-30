using eCommerceAPI.Data.PaymentTypes;
using eCommerceAPI.Data.Users;

namespace eCommerceAPI.Data.UserPaymentTypes
{
    public class UserPaymentMethod
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Provider { get; set; }
        public string AccountNumber { get; set; }
        public string ExpiryDate { get; set; }
        public Guid UserId { get; set; }
        public virtual User User { get; set; }
        public int PaymentTypeId { get; set; }
        public virtual PaymentType PaymentType { get; set; }
    }
}
