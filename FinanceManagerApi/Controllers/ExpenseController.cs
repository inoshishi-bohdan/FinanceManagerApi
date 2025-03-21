using AutoMapper;
using FinanceManagerApi.Data;
using FinanceManagerApi.Entities;
using FinanceManagerApi.Extensions;
using FinanceManagerApi.Models.Expense;
using FinanceManagerApi.Models.Response;
using FinanceManagerApi.Services.FieldValidationService;
using FinanceManagerApi.Services.UserService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinanceManagerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ExpenseController(IUserService userService, FinanceManagerDbContext dbContext, IMapper mapper) : ControllerBase
    {
        [HttpGet("getMyExpenses")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(UnauthorizedDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundDto))]
        public async Task<ActionResult<List<ExpenseDto>>> GetMyExpenses()
        {
            var myId = userService.GetMyId();

            if (myId == null)
            {
                return Unauthorized(new UnauthorizedDto { Message = "Couldn't get user id from http context" });
            }

            //check if user record exists
            if (!dbContext.Users.Any(user => user.Id == myId))
            {
                return NotFound(new NotFoundDto { Message = $"User with ID {myId} was not found" });
            }

            var expenses = await dbContext.Expenses
                .AsQueryable()
                .Include(expense => expense.Currency)
                .Include(expense => expense.ExpenseCategory)
                .Where(expense => expense.UserId == myId)
                .ToListAsync();
            var response = mapper.Map<List<ExpenseDto>>(expenses);

            return Ok(response);
        }

        [HttpPost("create")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(BadRequestDto))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(UnauthorizedDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundDto))]
        public async Task<ActionResult<ExpenseDto>> CreateExpense(CreateExpenseRequest request)
        {
            var validator = FieldValidationService.Create(request);

            validator
                .FieldIsRequired(x => x.Title).FieldHasMaxLength(x => x.Title, 250)
                .FieldIsRequired(x => x.Date)
                .FieldIsRequired(x => x.Amount)
                .FieldIsRequired(x => x.CurrencyId)
                .FieldIsRequired(x => x.ExpenseCategoryId);

            //check if request parameters is not null or missing
            if (validator.Any()) return validator.BadRequest();

            var myId = userService.GetMyId();

            if (myId == null)
            {
                return Unauthorized(new UnauthorizedDto { Message = "Couldn't get user id from http context" });
            }

            //check if user record exists
            if (!dbContext.Users.Any(user => user.Id == myId))
            {
                return NotFound(new NotFoundDto { Message = $"User with ID {myId} was not found" });
            }

            //check if specified currency is valid
            if (!dbContext.Currencies.Any(currency => currency.Id == request.CurrencyId))
            {
                return BadRequest(new BadRequestDto { Message = "Invalid request", Errors = new List<string> { $"Currency with ID {request.CurrencyId} was not found" } });
            }

            //check if specified expense category is valid 
            if (!dbContext.ExpenseCategories.Any(category => category.Id == request.ExpenseCategoryId))
            {
                return BadRequest(new BadRequestDto { Message = "Invalid request", Errors = new List<string> { $"Expense category with ID {request.ExpenseCategoryId} was not found" } });
            }

            //check if amount is not negative number
            if (request.Amount <= 0)
            {
                return BadRequest(new BadRequestDto { Message = "Invalid request", Errors = new List<string> { "Amount can not be less of equal to 0" } });
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
                return BadRequest(new BadRequestDto { Message = "Missing record", Errors = new List<string> { $"Expense record with ID {newExpense.Id} was not found" } });
            }

            var response = mapper.Map<ExpenseDto>(entry);

            return Ok(response);
        }

        [HttpPut("update/{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(BadRequestDto))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(UnauthorizedDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundDto))]
        public async Task<ActionResult<ExpenseDto>> UpdateExpense(int id, UpdateExpenseRequest request)
        {
            var validator = FieldValidationService.Create(request);

            validator
                .FieldIsRequired(x => x.Title).FieldHasMaxLength(x => x.Title, 250)
                .FieldIsRequired(x => x.Date)
                .FieldIsRequired(x => x.Amount)
                .FieldIsRequired(x => x.CurrencyId)
                .FieldIsRequired(x => x.ExpenseCategoryId);

            //check if request parameters is not null or missing
            if (validator.Any()) return validator.BadRequest();

            var myId = userService.GetMyId();

            if (myId == null)
            {
                return Unauthorized(new UnauthorizedDto { Message = "Couldn't get user id from http context" });
            }

            //check if user record exists
            if (!dbContext.Users.Any(user => user.Id == myId))
            {
                return NotFound(new NotFoundDto { Message = $"User with ID {myId} was not found" });
            }

            //check if specified currency is valid
            if (!dbContext.Currencies.Any(currency => currency.Id == request.CurrencyId))
            {
                return BadRequest(new BadRequestDto { Message = "Invalid request", Errors = new List<string> { $"Currency with ID {request.CurrencyId} was not found" } });
            }

            //check if specified expense category is valid 
            if (!dbContext.ExpenseCategories.Any(category => category.Id == request.ExpenseCategoryId))
            {
                return BadRequest(new BadRequestDto { Message = "Invalid request", Errors = new List<string> { $"Expense category with ID {request.ExpenseCategoryId} was not found" } });
            }

            //check if amount is not negative number
            if (request.Amount <= 0)
            {
                return BadRequest(new BadRequestDto { Message = "Invalid request", Errors = new List<string> { "Amount can not be less of equal to 0" } });
            }

            var amount = Math.Round((decimal)request.Amount!, 2, MidpointRounding.AwayFromZero);
            var entry = await dbContext.Expenses
                .AsQueryable()
                .FirstOrDefaultAsync(expense => expense.Id == id);

            if (entry == null)
            {
                return BadRequest(new BadRequestDto { Message = "Invalid request", Errors = new List<string> { $"Expense record with ID {id} was not found" } });
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
                return BadRequest(new BadRequestDto { Message = "Missing record", Errors = new List<string> { $"Expense record with ID {id} was not found" } });
            }

            var response = mapper.Map<ExpenseDto>(entry);


            return Ok(response);
        }

        [HttpDelete("delete/{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(BadRequestDto))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(UnauthorizedDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundDto))]
        public async Task<ActionResult<string>> DeleteExpense(int id)
        {
            var myId = userService.GetMyId();

            if (myId == null)
            {
                return Unauthorized(new UnauthorizedDto { Message = "Couldn't get user id from http context" });
            }

            //check if user record exists
            if (!dbContext.Users.Any(user => user.Id == myId))
            {
                return NotFound(new NotFoundDto { Message = $"User with ID {myId} was not found" });
            }

            var entry = await dbContext.Expenses
                .AsQueryable()
                .FirstOrDefaultAsync(expense => expense.Id == id);

            if (entry == null)
            {
                return BadRequest(new BadRequestDto { Message = "Invalid request", Errors = new List<string> { $"Expense record with ID {id} was not found" } });
            }

            dbContext.Expenses.Remove(entry);

            await dbContext.SaveChangesAsync();

            return Ok($"Expense record with ID {entry.Id} was deleted.");
        }
    }
}
