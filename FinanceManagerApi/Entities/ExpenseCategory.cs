using System.Text.Json.Serialization;

namespace FinanceManagerApi.Entities
{
    public class ExpenseCategory
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        [JsonIgnore]
        public ICollection<Expense> Expenses { get; set; } = new List<Expense>();
    }
}
