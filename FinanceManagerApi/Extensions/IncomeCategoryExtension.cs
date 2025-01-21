using FinanceManagerApi.Entities;
using FinanceManagerApi.Models.IncomeCategory;
using Microsoft.EntityFrameworkCore;

namespace FinanceManagerApi.Extensions
{
    public static class IncomeCategoryExtension
    {
        public static async Task<List<IncomeCategoryDto>> ToIncomeCategoryDtoListAsync(this IQueryable<IncomeCategory> incomeCategories)
        {
            return await incomeCategories.Select(incomeCategory => new IncomeCategoryDto
            {
                Id = incomeCategory.Id,
                Name = incomeCategory.Name,
            }).ToListAsync();
        }
    }
}

