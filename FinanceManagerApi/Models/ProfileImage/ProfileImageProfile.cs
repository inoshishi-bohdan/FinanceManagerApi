using AutoMapper;

namespace FinanceManagerApi.Models.ProfileImage
{
    public class ProfileImageProfile : Profile
    {
        public ProfileImageProfile()
        {
            CreateMap<Entities.ProfileImage, ProfileImageDto>();
        }
    }
}
