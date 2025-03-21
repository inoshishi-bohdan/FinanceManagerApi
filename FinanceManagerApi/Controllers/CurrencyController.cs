using AutoMapper;
using FinanceManagerApi.Data;
using FinanceManagerApi.Extensions;
using FinanceManagerApi.Models.Currency;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinanceManagerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurrencyController(FinanceManagerDbContext dbContext, IMapper mapper) : ControllerBase
    {
        [HttpGet("getList")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<CurrencyDto>>> GetAllCurrencies()
        {
            var currencies = await dbContext.Currencies.ToListAsync();
            var response =  mapper.Map<List<CurrencyDto>>(currencies);

            return Ok(response);
        }
    }
}
