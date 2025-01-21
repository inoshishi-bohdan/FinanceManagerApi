using FinanceManagerApi.Entities;
using FinanceManagerApi.Models.ExpenseCategory;
using Microsoft.EntityFrameworkCore;

namespace FinanceManagerApi.Extensions
{
    public static class ExpenseCategoryExtension
    {
        public static async Task<List<ExpenseCategoryDto>> ToExpenseCategoryDtoListAsync(this IQueryable<ExpenseCategory> expenseCategories)
        {
            return await expenseCategories.Select(expenseCategory => new ExpenseCategoryDto
            {
                Id = expenseCategory.Id,
                Name = expenseCategory.Name,
            }).ToListAsync();
        }
    }
}
