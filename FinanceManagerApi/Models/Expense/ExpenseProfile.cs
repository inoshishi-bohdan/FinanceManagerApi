using AutoMapper;

namespace FinanceManagerApi.Models.Expense
{
    public class ExpenseProfile : Profile
    {
        public ExpenseProfile()
        {
            CreateMap<Entities.Expense, ExpenseDto>()
                .ForMember(dest =>
                    dest.ExpenseCategory,
                    opt => opt.MapFrom(src => src.ExpenseCategory.Name))
                .ForMember(dest =>
                    dest.Currency,
                    opt => opt.MapFrom(src => src.Currency.Name));
        }
    }
}
