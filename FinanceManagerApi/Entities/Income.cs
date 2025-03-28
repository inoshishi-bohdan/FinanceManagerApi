﻿namespace FinanceManagerApi.Entities
{
    public class Income
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public DateOnly Date { get; set; }
        public decimal Amount { get; set; }
        public int IncomeCategoryId { get; set; }
        public IncomeCategory IncomeCategory { get; set; } = null!;
        public int CurrencyId { get; set; }
        public Currency Currency { get; set; } = null!;
        public int UserId { get; set; }
        public User User { get; set; } = null!;
    }
}
