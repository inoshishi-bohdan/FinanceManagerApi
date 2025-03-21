using System.ComponentModel.DataAnnotations;

namespace FinanceManagerApi.Enums
{
    public enum IncomeCategories
    {
        Salary = 1,
        Investments = 2,
        [Display(Name = "Business Income")]
        BusinessIncome = 3,
        [Display(Name = "Rental Income")]
        RentalIncome = 4,
        [Display(Name = "Other Income")]
        OtherIncome = 5,
    }
}
