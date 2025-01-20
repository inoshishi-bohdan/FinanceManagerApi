namespace FinanceManagerApi.Models.Auth
{
    public class RefreshTokenRequestDto
    {
        public int? UserId { get; set; }
        public string? RefreshToken { get; set; }
    }
}
