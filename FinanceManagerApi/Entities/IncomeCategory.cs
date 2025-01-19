using System.Text.Json.Serialization;

namespace FinanceManagerApi.Entities
{
    public class IncomeCategory
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        [JsonIgnore]
        public ICollection<Income> Incomes { get; set; } = new List<Income>();

    }
}
