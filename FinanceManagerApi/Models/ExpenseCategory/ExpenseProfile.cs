using AutoMapper;

namespace FinanceManagerApi.Models.ExpenseCategory
{
    public class ExpenseProfile: Profile
    {
        public ExpenseProfile()
        {
            CreateMap<Entities.ExpenseCategory, ExpenseCategoryDto>();
        }
    }
}
