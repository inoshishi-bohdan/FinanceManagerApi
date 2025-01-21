namespace FinanceManagerApi.Models.Expense
{
    public class ExpenseDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public DateOnly Date { get; set; }
        public decimal Amount { get; set; }
        public int ExpenseCategoryId { get; set; }
        public string ExpenseCategory { get; set; } = null!;
        public int CurrencyId { get; set; }
        public string Currency { get; set; } = null!;
    }
}
