namespace eCommerceAPI.Business.Users.Queries.GetAllUsers
{
    public class GetAllUsersListResponse
    {
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string AccountStatus { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string Country { get; set; }
        public string AddressLine { get; set; }
        public string PostalCode { get; set; }
    }
}
