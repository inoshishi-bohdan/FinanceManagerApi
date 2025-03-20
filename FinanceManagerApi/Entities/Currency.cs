namespace FinanceManagerApi.Entities
{
    public class Currency
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public ICollection<Expense> Expenses { get; set; } = new List<Expense>();
        public ICollection<Income> Incomes { get; set; } = new List<Income>();
    }
}
