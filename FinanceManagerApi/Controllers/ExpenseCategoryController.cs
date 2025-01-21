using FinanceManagerApi.Data;
using FinanceManagerApi.Extensions;
using FinanceManagerApi.Models.ExpenseCategory;
using Microsoft.AspNetCore.Mvc;

namespace FinanceManagerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpenseCategoryController(FinanceManagerDbContext dbContext) : ControllerBase
    {
        [HttpGet("getList")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<ExpenseCategoryDto>>> GetAllExpenseCategories()
        {
            var response = await dbContext.ExpenseCategories.ToExpenseCategoryDtoListAsync();

            return Ok(response);
        }
    }
}
