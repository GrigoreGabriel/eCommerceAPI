namespace eCommerceAPI.Business.Users.Commands.Create
{
    public class CreateUserRequest
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

    }
}
