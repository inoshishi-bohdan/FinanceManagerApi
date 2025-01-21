using FinanceManagerApi.Data;
using FinanceManagerApi.Entities;
using FinanceManagerApi.Extensions;
using FinanceManagerApi.Models.Expense;
using FinanceManagerApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinanceManagerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ExpenseController(IUserService userService, FinanceManagerDbContext dbContext) : ControllerBase
    {
        [HttpGet("getMyExpenses")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<List<ExpenseDto>>> GetMyExpenses()
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

            var response = await dbContext.Expenses
                .AsQueryable()
                .Include(expense => expense.Currency)
                .Include(expense => expense.ExpenseCategory)
                .Where(expense => expense.UserId == myId)
                .ToExpenseDtoListAsync();

            return Ok(response);
        }

        [HttpPost("create")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ExpenseDto>> CreateExpense(CreateExpenseRequestDto request)
        {
            var validator = FieldValidator.Create(request);

            validator
                .FieldIsRequired(x => x.Title)
                .FieldIsRequired(x => x.Date)
                .FieldIsRequired(x => x.Amount)
                .FieldIsRequired(x => x.CurrencyId)
                .FieldIsRequired(x => x.ExpenseCategoryId);

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

            //check if specified expense category is valid 
            if (!dbContext.ExpenseCategories.Any(category => category.Id == request.ExpenseCategoryId))
            {
                return BadRequest($"Expense category with ID {request.ExpenseCategoryId} was not found.");
            }

            //check if amount is not negative number
            if (request.Amount <= 0)
            {
                return BadRequest("Amount can not be less of equal to 0.");
            }

            var amount = Math.Round((decimal)request.Amount!, 2, MidpointRounding.AwayFromZero);

            var newExpense = new Expense
            {
                Title = request.Title!,
                Date = (DateOnly)request.Date!,
                Amount = amount,
                CurrencyId = (int)request.CurrencyId!,
                ExpenseCategoryId = (int)request.ExpenseCategoryId!,
                UserId = (int)myId
            };
            dbContext.Expenses.Add(newExpense);

            await dbContext.SaveChangesAsync();

            var entry = await dbContext.Expenses
                .AsQueryable()
                .Include(expense => expense.Currency)
                .Include(expense => expense.ExpenseCategory)
                .FirstOrDefaultAsync(expense => expense.Id == newExpense.Id);

            if (entry == null)
            {
                return BadRequest($"Expense record with ID {newExpense.Id} was not found.");
            }

            return Ok(entry.ToExpenseDto());
        }

        [HttpPut("update/{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ExpenseDto>> UpdateExpense(int id, UpdateExpenseRequestDto request)
        {
            var validator = FieldValidator.Create(request);

            validator
                .FieldIsRequired(x => x.Title)
                .FieldIsRequired(x => x.Date)
                .FieldIsRequired(x => x.Amount)
                .FieldIsRequired(x => x.CurrencyId)
                .FieldIsRequired(x => x.ExpenseCategoryId);

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

            //check if specified expense category is valid 
            if (!dbContext.ExpenseCategories.Any(category => category.Id == request.ExpenseCategoryId))
            {
                return BadRequest($"Expense category with ID {request.ExpenseCategoryId} was not found.");
            }

            //check if amount is not negative number
            if (request.Amount <= 0)
            {
                return BadRequest("Amount can not be less of equal to 0.");
            }

            var amount = Math.Round((decimal)request.Amount!, 2, MidpointRounding.AwayFromZero);
            var entry = await dbContext.Expenses
                .AsQueryable()
                .FirstOrDefaultAsync(expense => expense.Id == id);

            if (entry == null)
            {
                return BadRequest($"Expense record with ID {id} was not found.");
            }

            entry.Title = request.Title!;
            entry.Date = (DateOnly)request.Date!;
            entry.Amount = amount;
            entry.CurrencyId = (int)request.CurrencyId!;
            entry.ExpenseCategoryId = (int)request.ExpenseCategoryId!;

            await dbContext.SaveChangesAsync();

            entry = await dbContext.Expenses
                .AsQueryable()
                .Include(expense => expense.Currency)
                .Include(expense => expense.ExpenseCategory)
                .FirstOrDefaultAsync(expense => expense.Id == id);

            if (entry == null)
            {
                return BadRequest($"Expense record with ID {id} was not found.");
            }

            return Ok(entry.ToExpenseDto());
        }

        [HttpDelete("delete/{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<string>> DeleteExpense(int id)
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

            var entry = await dbContext.Expenses
                .AsQueryable()
                .FirstOrDefaultAsync(expense => expense.Id == id);

            if (entry == null)
            {
                return BadRequest($"Expense record with ID {id} was not found.");
            }

            dbContext.Expenses.Remove(entry);

            await dbContext.SaveChangesAsync();

            return Ok($"Expense record with ID {entry.Id} was deleted.");
        }
    }
}
