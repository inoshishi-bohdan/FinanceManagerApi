using AutoMapper;

namespace FinanceManagerApi.Models.User
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<Entities.User, UserDto>();
        }
    }
}
