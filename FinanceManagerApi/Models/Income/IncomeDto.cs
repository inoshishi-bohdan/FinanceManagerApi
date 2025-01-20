using FinanceManagerApi.Entities;

namespace FinanceManagerApi.Models.Income
{
    public class IncomeDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public DateOnly Date { get; set; }
        public decimal Amount { get; set; }
        public int IncomeCategoryId { get; set; }
        public string IncomeCategory { get; set; } = null!;
        public int CurrencyId { get; set; }
        public string Currency { get; set; } = null!;
    }
}
