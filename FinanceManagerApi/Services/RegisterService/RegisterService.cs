using FinanceManagerApi.Data;
using FinanceManagerApi.Entities;
using FinanceManagerApi.Extensions;
using FinanceManagerApi.Models.Register;
using FinanceManagerApi.Models.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FinanceManagerApi.Services.RegisterService
{
    public class RegisterService(FinanceManagerDbContext context) : IRegisterService
    {
        public async Task<UserDto?> RegisterAsync(RegisterRequest request)
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
    }
}
