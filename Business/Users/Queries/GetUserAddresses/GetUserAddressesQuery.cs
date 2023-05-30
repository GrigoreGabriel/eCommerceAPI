namespace eCommerceAPI.Business.Users.Queries.GetUserAddresses
{
    public class GetUserAddressesQuery
    {
        public Guid UserUID { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string AddressLine { get; set; }
        public string PostalCode { get; set; }
    }
}
