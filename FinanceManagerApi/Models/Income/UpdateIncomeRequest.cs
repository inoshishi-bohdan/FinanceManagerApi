namespace FinanceManagerApi.Models.Income
{
    public class UpdateIncomeRequest
    {
        public string? Title { get; set; }
        public DateOnly? Date { get; set; }
        public decimal? Amount { get; set; }
        public int? IncomeCategoryId { get; set; }
        public int? CurrencyId { get; set; }
    }
}
