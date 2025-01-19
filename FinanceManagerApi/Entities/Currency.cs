using System.Text.Json.Serialization;

namespace FinanceManagerApi.Entities
{
    public class Currency
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        [JsonIgnore]
        public ICollection<Expense> Expenses { get; set; } = new List<Expense>();
        [JsonIgnore]
        public ICollection<Income> Incomes { get; set; } = new List<Income>();
    }
}
