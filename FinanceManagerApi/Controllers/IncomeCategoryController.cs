using AutoMapper;
using FinanceManagerApi.Data;
using FinanceManagerApi.Extensions;
using FinanceManagerApi.Models.IncomeCategory;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinanceManagerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IncomeCategoryController(FinanceManagerDbContext dbContext, IMapper mapper) : ControllerBase
    {
        [HttpGet("getList")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<IncomeCategoryDto>>> GetAllIncomeCategories()
        {
            var incomeCategories  = await dbContext.IncomeCaregories.ToListAsync();
            var response = mapper.Map<List<IncomeCategoryDto>>(incomeCategories);

            return Ok(response);
        }
    }
}
