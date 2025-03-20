namespace FinanceManagerApi.Models.User
{
    public class UpdateRequest
    {
        public string? UserName { get; set; } 
        public string? Email { get; set; } 
        public string? OldPassword { get; set; }
        public string? NewPassword { get; set; }
        public int? ProfileImageId { get; set; }
    }
}
