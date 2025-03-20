namespace FinanceManagerApi.Entities
{
    public class IncomeCategory
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public ICollection<Income> Incomes { get; set; } = new List<Income>();

    }
}
