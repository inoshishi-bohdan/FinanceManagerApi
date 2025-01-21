using FinanceManagerApi.Data;
using FinanceManagerApi.Entities;
using FinanceManagerApi.Extensions;
using FinanceManagerApi.Models.Income;
using FinanceManagerApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinanceManagerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class IncomeController(IUserService userService, FinanceManagerDbContext dbContext) : ControllerBase
    {
        [HttpGet("getMyIncomes")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<List<IncomeDto>>> GetMyIncomes()
        {
            var myId = userService.GetMyId();

            if (myId == null)
            {
                return Unauthorized("Couldn't get user id from http context.");
            }

            //check if user record exists
            if (!dbContext.Users.Any(user => user.Id == myId))
            {
                return BadRequest($"User with ID {myId} was not found.");
            }

            var response = await dbContext.Incomes
                .AsQueryable()
                .Include(income => income.Currency)
                .Include(income => income.IncomeCategory)
                .Where(income => income.UserId == myId)
                .ToIncomeDtoListAsync();

            return Ok(response);
        }

        [HttpPost("create")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<IncomeDto>> CreateIncome(CreateIncomeRequestDto request)
        {
            var validator = FieldValidator.Create(request);

            validator
                .FieldIsRequired(x => x.Title)
                .FieldIsRequired(x => x.Date)
                .FieldIsRequired(x => x.Amount)
                .FieldIsRequired(x => x.CurrencyId)
                .FieldIsRequired(x => x.IncomeCategoryId);

            //check if request parameters is not null or missing
            if (validator.Any()) return validator.BadRequest();

            var myId = userService.GetMyId();

            if (myId == null)
            {
                return Unauthorized("Couldn't get user id from http context.");
            }

            //check if user record exists
            if (!dbContext.Users.Any(user => user.Id == myId))
            {
                return BadRequest($"User with ID {myId} was not found.");
            }

            //check if specified currency is valid
            if (!dbContext.Currencies.Any(currency => currency.Id == request.CurrencyId))
            {
                return BadRequest($"Currency with ID {request.CurrencyId} was not found.");
            }

            //check if specified income category is valid 
            if (!dbContext.IncomeCaregories.Any(category => category.Id == request.IncomeCategoryId))
            {
                return BadRequest($"Income category with ID {request.IncomeCategoryId} was not found.");
            }

            //check if amount is not negative number
            if (request.Amount <= 0)
            {
                return BadRequest("Amount can not be less of equal to 0.");
            }

            var amount = Math.Round((decimal)request.Amount!, 2, MidpointRounding.AwayFromZero);

            var newIncome = new Income
            {
                Title = request.Title!,
                Date = (DateOnly)request.Date!,
                Amount = amount,
                CurrencyId = (int)request.CurrencyId!,
                IncomeCategoryId = (int)request.IncomeCategoryId!,
                UserId = (int)myId
            };
            dbContext.Incomes.Add(newIncome);

            await dbContext.SaveChangesAsync();

            var entry = await dbContext.Incomes
                .AsQueryable()
                .Include(income => income.Currency)
                .Include(income => income.IncomeCategory)
                .FirstOrDefaultAsync(income => income.Id == newIncome.Id);

            if (entry == null)
            {
                return BadRequest($"Income record with ID {newIncome.Id} was not found.");
            }

            return Ok(entry.ToIncomeDto());
        }

        [HttpPut("update/{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<IncomeDto>> UpdateIncome(int id, UpdateIncomeRequestDto request)
        {
            var validator = FieldValidator.Create(request);

            validator
                .FieldIsRequired(x => x.Title)
                .FieldIsRequired(x => x.Date)
                .FieldIsRequired(x => x.Amount)
                .FieldIsRequired(x => x.CurrencyId)
                .FieldIsRequired(x => x.IncomeCategoryId);

            //check if request parameters is not null or missing
            if (validator.Any()) return validator.BadRequest();

            var myId = userService.GetMyId();

            if (myId == null)
            {
                return Unauthorized("Couldn't get user id from http context.");
            }

            //check if user record exists
            if (!dbContext.Users.Any(user => user.Id == myId))
            {
                return BadRequest($"User with ID {myId} was not found.");
            }

            //check if specified currency is valid
            if (!dbContext.Currencies.Any(currency => currency.Id == request.CurrencyId))
            {
                return BadRequest($"Currency with ID {request.CurrencyId} was not found.");
            }

            //check if specified income category is valid 
            if (!dbContext.IncomeCaregories.Any(category => category.Id == request.IncomeCategoryId))
            {
                return BadRequest($"Income category with ID {request.IncomeCategoryId} was not found.");
            }

            //check if amount is not negative number
            if (request.Amount <= 0)
            {
                return BadRequest("Amount can not be less of equal to 0.");
            }

            var amount = Math.Round((decimal)request.Amount!, 2, MidpointRounding.AwayFromZero);
            var entry = await dbContext.Incomes
                .AsQueryable()
                .FirstOrDefaultAsync(income => income.Id == id);

            if (entry == null)
            {
                return BadRequest($"Income record with ID {id} was not found.");
            }

            entry.Title = request.Title!;
            entry.Date = (DateOnly)request.Date!;
            entry.Amount = amount;
            entry.CurrencyId = (int)request.CurrencyId!;
            entry.IncomeCategoryId = (int)request.IncomeCategoryId!;

            await dbContext.SaveChangesAsync();

            entry = await dbContext.Incomes
                .AsQueryable()
                .Include(income => income.Currency)
                .Include(income => income.IncomeCategory)
                .FirstOrDefaultAsync(income => income.Id == id);

            if (entry == null)
            {
                return BadRequest($"Income record with ID {id} was not found.");
            }

            return Ok(entry.ToIncomeDto());
        }

        [HttpDelete("delete/{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<string>> DeleteIncome(int id)
        {
            var myId = userService.GetMyId();

            if (myId == null)
            {
                return Unauthorized("Couldn't get user id from http context.");
            }

            //check if user record exists
            if (!dbContext.Users.Any(user => user.Id == myId))
            {
                return BadRequest($"User with ID {myId} was not found.");
            }

            var entry = await dbContext.Incomes
                .AsQueryable()
                .FirstOrDefaultAsync(income => income.Id == id);

            if (entry == null)
            {
                return BadRequest($"Income record with ID {id} was not found.");
            }

            dbContext.Incomes.Remove(entry);

            await dbContext.SaveChangesAsync();

            return Ok($"Income record with ID {entry.Id} was deleted."); 
        }
    }
}
