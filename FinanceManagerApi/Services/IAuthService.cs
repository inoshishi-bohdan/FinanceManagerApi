using FinanceManagerApi.Models.Auth;
using FinanceManagerApi.Models.User;

namespace FinanceManagerApi.Services
{
    public interface IAuthService
    {
        Task<UserDto?> RegisterAsync(RegisterRequestDto request);
        Task<TokenResponseDto?> LoginAsync(LoginRequestDto request);
        Task<TokenResponseDto?> RefreshTokensAsync(RefreshTokenRequestDto request);
        bool IsValidEmail(string email);
    }
}
