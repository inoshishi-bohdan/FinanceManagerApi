using FinanceManagerApi.Entities;
using FinanceManagerApi.Models.Expense;
using Microsoft.EntityFrameworkCore;

namespace FinanceManagerApi.Extensions
{
    public static class ExpenseExtension
    {
        public static async Task<List<ExpenseDto>> ToExpenseDtoListAsync(this IQueryable<Expense> expenses)
        {
            return await expenses.Select(expense => new ExpenseDto
            {
                Id = expense.Id,
                Title = expense.Title,
                Date = expense.Date,
                Amount = expense.Amount,
                Currency = expense.Currency.Name,
                CurrencyId = expense.CurrencyId,
                ExpenseCategory = expense.ExpenseCategory.Name,
                ExpenseCategoryId = expense.ExpenseCategoryId,
            }).ToListAsync();
        }

        public static ExpenseDto ToExpenseDto(this Expense entry)
        {
            return new ExpenseDto
            {
                Id = entry.Id,
                Title = entry.Title,
                Date = entry.Date,
                Amount = entry.Amount,
                CurrencyId = entry.CurrencyId,
                Currency = entry.Currency.Name,
                ExpenseCategoryId = entry.ExpenseCategoryId,
                ExpenseCategory = entry.ExpenseCategory.Name
            };
        }
    }
}
