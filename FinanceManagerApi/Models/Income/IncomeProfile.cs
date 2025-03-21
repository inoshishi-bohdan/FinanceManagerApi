using AutoMapper;

namespace FinanceManagerApi.Models.Income
{
    public class IncomeProfile : Profile
    {
        public IncomeProfile()
        {
            CreateMap<Entities.Income, IncomeDto>()
                .ForMember(dest =>
                    dest.IncomeCategory,
                    opt => opt.MapFrom(src => src.IncomeCategory.Name))
                .ForMember(dest =>
                    dest.Currency,
                    opt => opt.MapFrom(src => src.Currency.Name));
        }
    }
}
