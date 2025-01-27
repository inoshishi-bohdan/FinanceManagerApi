using FinanceManagerApi.Data;
using FinanceManagerApi.Entities;
using FinanceManagerApi.Extensions;
using FinanceManagerApi.Models.Auth;
using FinanceManagerApi.Models.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace FinanceManagerApi.Services
{
    public class AuthService(FinanceManagerDbContext context) : IAuthService
    {
        public async Task<TokenResponseDto?> LoginAsync(LoginRequestDto request)
        {
            var user = await context.Users.AsQueryable().FirstOrDefaultAsync(user => user.Email == request.Email!.ToLower());

            if (user == null)
            {
                return null;
            }

            if (new PasswordHasher<User>().VerifyHashedPassword(user, user.PasswordHash, request.Password!) == PasswordVerificationResult.Failed)
            {
                return null;
            }

            return await CreateTokenResponse(user); ;
        }

        public async Task<UserDto?> RegisterAsync(RegisterRequestDto request)
        {
            if (await context.Users.AsQueryable().AnyAsync(user => user.Email == request.Email!.ToLower()))
            {
                return null;
            }

            var user = new User();
            var hashedPassword = new PasswordHasher<User>().HashPassword(user, request.Password!);
            user.UserName = request.UserName!;
            user.Email = request.Email!.ToLower();
            user.PasswordHash = hashedPassword;
            user.ProfileImageId = 1;
            context.Users.Add(user);
            await context.SaveChangesAsync();

            return user.ToUserDto();
        }

        private string CreateToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("TOKEN")!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);
            var tokenDescriptor = new JwtSecurityToken(
                issuer: Environment.GetEnvironmentVariable("ISSUER")!,
                audience: Environment.GetEnvironmentVariable("AUDIENCE"),
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(15),
                signingCredentials: creds
                );

            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }

        private async Task<TokenResponseDto> CreateTokenResponse(User user)
        {
            return new TokenResponseDto { AccessToken = CreateToken(user), RefreshToken = await GenerateAndSaveRefreshTokenAsync(user) };
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);

            return Convert.ToBase64String(randomNumber);
        }

        private async Task<string> GenerateAndSaveRefreshTokenAsync(User user)
        {
            var refreshToken = GenerateRefreshToken();
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            await context.SaveChangesAsync();

            return refreshToken;
        }

        private async Task<User?> ValidateRefreshTokenAsync(int userId, string refreshToken)
        {
            var user = await context.Users.FindAsync(userId);

            if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                return null;
            }

            return user;
        }

        public async Task<TokenResponseDto?> RefreshTokensAsync(RefreshTokenRequestDto request)
        {
            var user = await ValidateRefreshTokenAsync((int)request.UserId!, request.RefreshToken!);

            if (user == null)
            {
                return null;
            }

            return await CreateTokenResponse(user);
        }
    }
}
