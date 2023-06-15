using eCommerceAPI.Data.Users;
using System.ComponentModel.DataAnnotations.Schema;

namespace eCommerceAPI.Data.Addresses
{
    public class Address
    {
        public int Id { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string AddressLine { get; set; }
        public string PostalCode { get; set; }
        public string PhoneNumber { get; set; }
        public string Country { get; set; }

        public virtual User User { get; set; }

        [ForeignKey("UserId")]
        public Guid? UserId { get; set; }

    }
}
