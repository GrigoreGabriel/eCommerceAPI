namespace eCommerceAPI.Business.Users.Commands.Update
{
    public class UpdateUserInfoRequest
    {
        public Guid UserId { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }
        public string? Region { get; set; }
        public string? AddressLine { get; set; }
        public string? UserPhoneNumber { get; set; }
        public string? PostalCode { get; set; }
    }
}
