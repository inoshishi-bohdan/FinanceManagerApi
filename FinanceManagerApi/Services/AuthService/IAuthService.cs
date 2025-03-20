using FinanceManagerApi.Models.Auth;

namespace FinanceManagerApi.Services.AuthService
{
    public interface IAuthService
    {
        Task<TokenResponse?> LoginAsync(LoginRequest request);
        Task<TokenResponse?> RefreshTokensAsync(RefreshTokenRequest request);
    }
}
