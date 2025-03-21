using FinanceManagerApi.Entities;
using FinanceManagerApi.Enums;
using FinanceManagerApi.Models.Statistic;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace FinanceManagerApi.Extensions
{
    public static class StatisticExtension
    {
        private static readonly decimal EURToUSDRate = 1.06m;
        private static readonly decimal USDToEURRate = 0.94m;

        public static async Task<List<StatisticItemDto>> ToEURStatisticDataAsync(this IQueryable<Income> incomes)
        {
            var transformedIncomes = await incomes.Select(income => new
            {
                income.Date.Month,
                Amount = (Currencies)income.CurrencyId == Currencies.USD ? Math.Round(income.Amount * USDToEURRate, 2) : income.Amount
            }).ToListAsync();

            var monthIncomes = transformedIncomes.GroupBy(income => income.Month, (key, g) => new
            {
                Month = DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(key),
                TotalAmount = g.Sum(income => income.Amount)
            })
            .ToList();

            var result = new List<StatisticItemDto>
            {
                new() { Month = "Jan", TotalAmount = 0 },
                new() { Month = "Feb", TotalAmount = 0 },
                new() { Month = "Mar", TotalAmount = 0 },
                new() { Month = "Apr", TotalAmount = 0 },
                new() { Month = "May", TotalAmount = 0 },
                new() { Month = "Jun", TotalAmount = 0 },
                new() { Month = "Jul", TotalAmount = 0 },
                new() { Month = "Aug", TotalAmount = 0 },
                new() { Month = "Sep", TotalAmount = 0 },
                new() { Month = "Oct", TotalAmount = 0 },
                new() { Month = "Nov", TotalAmount = 0 },
                new() { Month = "Dec", TotalAmount = 0 },
            };

            foreach (var monthIncome in monthIncomes)
            {
                var statisticItem = result.Find(item => item.Month == monthIncome.Month);
                statisticItem!.TotalAmount = monthIncome.TotalAmount;
            }

            return result;
        }

        public static async Task<List<StatisticItemDto>> ToUSDStatisticDataAsync(this IQueryable<Income> incomes)
        {
            var transformedIncomes = await incomes.Select(income => new
            {
                income.Date.Month,
                Amount = (Currencies)income.CurrencyId == Currencies.EUR ? Math.Round(income.Amount * EURToUSDRate, 2) : income.Amount
            }).ToListAsync();

            var monthIncomes = transformedIncomes.GroupBy(income => income.Month, (key, g) => new
            {
                Month = DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(key),
                TotalAmount = g.Sum(income => income.Amount)
            })
            .ToList();

            var result = new List<StatisticItemDto>
            {
                new() { Month = "Jan", TotalAmount = 0 },
                new() { Month = "Feb", TotalAmount = 0 },
                new() { Month = "Mar", TotalAmount = 0 },
                new() { Month = "Apr", TotalAmount = 0 },
                new() { Month = "May", TotalAmount = 0 },
                new() { Month = "Jun", TotalAmount = 0 },
                new() { Month = "Jul", TotalAmount = 0 },
                new() { Month = "Aug", TotalAmount = 0 },
                new() { Month = "Sep", TotalAmount = 0 },
                new() { Month = "Oct", TotalAmount = 0 },
                new() { Month = "Nov", TotalAmount = 0 },
                new() { Month = "Dec", TotalAmount = 0 },
            };

            foreach (var monthIncome in monthIncomes)
            {
                var statisticItem = result.Find(item => item.Month == monthIncome.Month);
                statisticItem!.TotalAmount = monthIncome.TotalAmount;
            }

            return result;
        }

        public static async Task<List<StatisticItemDto>> ToEURStatisticDataAsync(this IQueryable<Expense> expenses)
        {
            var transformedExpenses = await expenses.Select(expense => new
            {
                expense.Date.Month,
                Amount = (Currencies)expense.CurrencyId == Currencies.USD ? Math.Round(expense.Amount * USDToEURRate, 2) : expense.Amount
            }).ToListAsync();

            var monthExpenses = transformedExpenses.GroupBy(expense => expense.Month, (key, g) => new
            {
                Month = DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(key),
                TotalAmount = g.Sum(expense => expense.Amount)
            })
            .ToList();

            var result = new List<StatisticItemDto>
            {
                new() { Month = "Jan", TotalAmount = 0 },
                new() { Month = "Feb", TotalAmount = 0 },
                new() { Month = "Mar", TotalAmount = 0 },
                new() { Month = "Apr", TotalAmount = 0 },
                new() { Month = "May", TotalAmount = 0 },
                new() { Month = "Jun", TotalAmount = 0 },
                new() { Month = "Jul", TotalAmount = 0 },
                new() { Month = "Aug", TotalAmount = 0 },
                new() { Month = "Sep", TotalAmount = 0 },
                new() { Month = "Oct", TotalAmount = 0 },
                new() { Month = "Nov", TotalAmount = 0 },
                new() { Month = "Dec", TotalAmount = 0 },
            };

            foreach (var monthExpense in monthExpenses)
            {
                var statisticItem = result.Find(item => item.Month == monthExpense.Month);
                statisticItem!.TotalAmount = monthExpense.TotalAmount;
            }

            return result;
        }

        public static async Task<List<StatisticItemDto>> ToUSDStatisticDataAsync(this IQueryable<Expense> expenses)
        {
            var transformedExpenses = await expenses.Select(expense => new
            {
                expense.Date.Month,
                Amount = (Currencies)expense.CurrencyId == Currencies.EUR ? Math.Round(expense.Amount * EURToUSDRate, 2) : expense.Amount
            }).ToListAsync();

            var monthExpenses = transformedExpenses.GroupBy(expense => expense.Month, (key, g) => new
            {
                Month = DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(key),
                TotalAmount = g.Sum(expense => expense.Amount)
            })
            .ToList();

            var result = new List<StatisticItemDto>
            {
                new() { Month = "Jan", TotalAmount = 0 },
                new() { Month = "Feb", TotalAmount = 0 },
                new() { Month = "Mar", TotalAmount = 0 },
                new() { Month = "Apr", TotalAmount = 0 },
                new() { Month = "May", TotalAmount = 0 },
                new() { Month = "Jun", TotalAmount = 0 },
                new() { Month = "Jul", TotalAmount = 0 },
                new() { Month = "Aug", TotalAmount = 0 },
                new() { Month = "Sep", TotalAmount = 0 },
                new() { Month = "Oct", TotalAmount = 0 },
                new() { Month = "Nov", TotalAmount = 0 },
                new() { Month = "Dec", TotalAmount = 0 },
            };

            foreach (var monthExpense in monthExpenses)
            {
                var statisticItem = result.Find(item => item.Month == monthExpense.Month);
                statisticItem!.TotalAmount = monthExpense.TotalAmount;
            }

            return result;
        }

        public static async Task<List<DistributionItemDto>> ToDistributionDataAsync(this IQueryable<Income> incomes)
        {
            var result = await incomes
                .GroupBy(income => income.IncomeCategoryId, (key, g) => new DistributionItemDto { CategoryName = ((IncomeCategories)key).GetDisplayAsOrName(), RecordCount = g.Count() })
                .ToListAsync();
            
            return result;
        }

        public static async Task<List<DistributionItemDto>> ToDistributionDataAsync(this IQueryable<Expense> expenses)
        {
            var result = await expenses
                .GroupBy(expense => expense.ExpenseCategoryId, (key, g) => new DistributionItemDto { CategoryName = ((ExpenseCategories)key).GetDisplayAsOrName(), RecordCount = g.Count() })
                .ToListAsync();

            return result;
        }
    }
}
