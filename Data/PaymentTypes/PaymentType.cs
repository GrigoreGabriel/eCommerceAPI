using eCommerceAPI.Data.UserPaymentTypes;
using System.ComponentModel.DataAnnotations;

namespace eCommerceAPI.Data.PaymentTypes
{
    public class PaymentType
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<UserPaymentMethod> UserPaymentMethods { get; set; }
    }
}
