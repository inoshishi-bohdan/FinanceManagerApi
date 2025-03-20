using System.Security.Claims;

namespace FinanceManagerApi.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public int? GetMyId()
        {
            var claimValue = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (claimValue == null || !int.TryParse(claimValue, out int result))
            {
                return null;
            }

            return result;
        }
    }
}
