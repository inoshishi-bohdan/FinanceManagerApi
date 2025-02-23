using FinanceManager.Util;

namespace FinanceManagerApi.Enums
{
    public enum IncomeCategories
    {
        Salary = 1,
        Investments = 2,
        [DisplayAs("Business Income")]
        BusinessIncome = 3,
        [DisplayAs("Rental Income")]
        RentalIncome = 4,
        [DisplayAs("Other Income")]
        OtherIncome = 5,
    }
}
