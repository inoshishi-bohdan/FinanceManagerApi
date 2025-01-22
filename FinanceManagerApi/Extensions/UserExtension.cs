using FinanceManagerApi.Entities;
using FinanceManagerApi.Models.User;

namespace FinanceManagerApi.Extensions
{
    public static class UserExtension
    {
        public static UserDto ToUserDto(this User entry)
        {
            return new UserDto
            {
                UserName = entry.UserName,
                Email = entry.Email,
                Password = entry.PasswordHash,
                ProfileImageId = entry.ProfileImageId
            };
        }
    }
}
