using AutoMapper;
using FinanceManagerApi.Data;
using FinanceManagerApi.Extensions;
using FinanceManagerApi.Models.ExpenseCategory;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinanceManagerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpenseCategoryController(FinanceManagerDbContext dbContext, IMapper mapper) : ControllerBase
    {
        [HttpGet("getList")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<ExpenseCategoryDto>>> GetAllExpenseCategories()
        {
            var expenseCategories = await dbContext.ExpenseCategories.ToListAsync();
            var response = mapper.Map<List<ExpenseCategoryDto>>(expenseCategories);

            return Ok(response);
        }
    }
}
