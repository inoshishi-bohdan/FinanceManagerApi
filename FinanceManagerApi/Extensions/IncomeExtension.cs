using FinanceManagerApi.Entities;
using FinanceManagerApi.Models.Income;
using Microsoft.EntityFrameworkCore;

namespace FinanceManagerApi.Extensions
{
    public static class IncomeExtension
    {
        public static async Task<List<IncomeDto>> ToIncomeDtoListAsync(this IQueryable<Income> incomes) 
        {
            return await incomes.Select(income => new IncomeDto 
            {
                Id = income.Id,
                Title = income.Title,
                Date = income.Date,
                Amount = income.Amount,
                Currency = income.Currency.Name,
                CurrencyId = income.CurrencyId,
                IncomeCategory = income.IncomeCategory.Name,
                IncomeCategoryId = income.IncomeCategoryId,
            }).ToListAsync();
        }

        public static IncomeDto ToIncomeDto(this Income entry)
        {
            return new IncomeDto
            {
                Id = entry.Id,
                Title = entry.Title,
                Date = entry.Date,
                Amount = entry.Amount,
                CurrencyId = entry.CurrencyId,
                Currency = entry.Currency.Name,
                IncomeCategoryId = entry.IncomeCategoryId,
                IncomeCategory = entry.IncomeCategory.Name
            };
        }
    }
}
