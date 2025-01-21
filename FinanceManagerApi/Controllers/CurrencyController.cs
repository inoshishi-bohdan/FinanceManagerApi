using FinanceManagerApi.Data;
using FinanceManagerApi.Extensions;
using FinanceManagerApi.Models.Currency;
using Microsoft.AspNetCore.Mvc;

namespace FinanceManagerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurrencyController(FinanceManagerDbContext dbContext) : ControllerBase
    {
        [HttpGet("getList")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<CurrencyDto>>> GetAllCurrencies()
        {
            var response = await dbContext.Currencies.ToCurrencyDtoListAsync();

            return Ok(response);
        }
    }
}
