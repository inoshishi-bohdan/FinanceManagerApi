using AutoMapper;

namespace FinanceManagerApi.Models.IncomeCategory
{
    public class IncomeCategoryProfile : Profile
    {
        public IncomeCategoryProfile()
        {
            CreateMap<Entities.IncomeCategory, IncomeCategoryDto>();
        }
    }
}
