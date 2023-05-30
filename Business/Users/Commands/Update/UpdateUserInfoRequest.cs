namespace eCommerceAPI.Business.Users.Commands.Update
{
    public class UpdateUserInfoRequest
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string AddressLine { get; set; }
        public string PostalCode { get; set; }
    }
}
