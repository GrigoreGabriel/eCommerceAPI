namespace eCommerceAPI.Business.Addresses.Commands.CreateAddress
{
    public class CreateAddressRequest
    {
        public string City { get; set; }
        public string Region { get; set; }
        public string AddressLine { get; set; }
        public string PostalCode { get; set; }
    }
}
