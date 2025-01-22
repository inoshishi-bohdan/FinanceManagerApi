namespace FinanceManagerApi.Models.User
{
    public class UserDto
    {
        public string UserName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public int ProfileImageId { get; set; }
    }
}
