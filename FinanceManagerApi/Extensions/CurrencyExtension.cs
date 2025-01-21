using FinanceManagerApi.Entities;
using FinanceManagerApi.Models.Currency;
using Microsoft.EntityFrameworkCore;

namespace FinanceManagerApi.Extensions
{
    public static class CurrencyExtension
    {
        public static async Task<List<CurrencyDto>> ToCurrencyDtoListAsync(this IQueryable<Currency> currencies)
        {
            return await currencies.Select(currency => new CurrencyDto
            {
                Id = currency.Id,
                Name = currency.Name,
            }).ToListAsync();
        }
    }
}
