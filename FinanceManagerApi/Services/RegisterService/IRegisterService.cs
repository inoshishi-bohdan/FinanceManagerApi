using FinanceManagerApi.Models.Register;
using FinanceManagerApi.Models.User;

namespace FinanceManagerApi.Services.RegisterService
{
    public interface IRegisterService
    {
        Task<UserDto?> RegisterAsync(RegisterRequest request);
    }
}
