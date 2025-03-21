namespace FinanceManagerApi.Models.Expense
{
    public class CreateRequest
    {
        public string? Title { get; set; }
        public DateOnly? Date { get; set; }
        public decimal? Amount { get; set; }
        public int? ExpenseCategoryId { get; set; }
        public int? CurrencyId { get; set; }
    }
}
