using FinanceManagerApi.Data;
using FinanceManagerApi.Extensions;
using FinanceManagerApi.Models.IncomeCategory;
using Microsoft.AspNetCore.Mvc;

namespace FinanceManagerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IncomeCategoryController(FinanceManagerDbContext dbContext) : ControllerBase
    {
        [HttpGet("getList")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<IncomeCategoryDto>>> GetAllIncomeCategories()
        {
            var response = await dbContext.IncomeCaregories.ToIncomeCategoryDtoListAsync();

            return Ok(response);
        }
    }
}
