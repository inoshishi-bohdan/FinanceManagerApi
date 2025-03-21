using FinanceManagerApi.Data;
using FinanceManagerApi.Enums;
using FinanceManagerApi.Extensions;
using FinanceManagerApi.Models.Response;
using FinanceManagerApi.Models.Statistic;
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
    public class StatisticController(IUserService userService, FinanceManagerDbContext dbContext) : ControllerBase
    {
        [HttpPost("income")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(BadRequestDto))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(UnauthorizedDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundDto))]
        public async Task<ActionResult<List<StatisticItemDto>>> GetMyIncomeStatistic(GetStatisticRequest request)
        {
            var validator = FieldValidationService.Create(request);

            validator
                .FieldIsRequired(x => x.Year)
                .FieldIsRequired(x => x.CurrencyId);

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
                return BadRequest(new BadRequestDto { Message = "Invalid request", Errors = [$"Currency with ID {request.CurrencyId} was not found"] });
            }

            var selectedCurrency = (Currencies)request.CurrencyId!;
            var query = dbContext.Incomes.AsQueryable().Where(income => income.Date.Year == request.Year && income.UserId == myId);
            List<StatisticItemDto> response;

            if (selectedCurrency == Currencies.USD)
            {
                response = await query.ToUSDStatisticDataAsync();
            }
            else
            {
                response = await query.ToEURStatisticDataAsync();
            }

            return Ok(response);
        }

        [HttpGet("getIncomeRecordPeriod")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(UnauthorizedDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundDto))]
        public async Task<ActionResult<List<int>>> GetMyIncomeRecordPeriod()
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

            var response = await dbContext.Incomes.AsQueryable().Where(income => income.UserId == myId).Select(income => income.Date.Year).Distinct().OrderBy(year => year).ToListAsync();

            return Ok(response);
        }

        [HttpPost("expense")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(BadRequestDto))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(UnauthorizedDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundDto))]
        public async Task<ActionResult<List<StatisticItemDto>>> GetMyExpenseStatistic(GetStatisticRequest request)
        {
            var validator = FieldValidationService.Create(request);

            validator
                .FieldIsRequired(x => x.Year)
                .FieldIsRequired(x => x.CurrencyId);

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
                return BadRequest(new BadRequestDto { Message = "Invalid request", Errors = [$"Currency with ID {request.CurrencyId} was not found"] });
            }

            var selectedCurrency = (Currencies)request.CurrencyId!;
            var query = dbContext.Expenses.AsQueryable().Where(expense => expense.Date.Year == request.Year && expense.UserId == myId);
            List<StatisticItemDto> response;

            if (selectedCurrency == Currencies.USD)
            {
                response = await query.ToUSDStatisticDataAsync();
            }
            else
            {
                response = await query.ToEURStatisticDataAsync();
            }

            return Ok(response);
        }

        [HttpGet("getExpenseRecordPeriod")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(UnauthorizedDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundDto))]
        public async Task<ActionResult<List<int>>> GetMyExpenseRecordPeriod()
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

            var response = await dbContext.Expenses.AsQueryable().Where(expense => expense.UserId == myId).Select(expense => expense.Date.Year).Distinct().OrderBy(year => year).ToListAsync();

            return Ok(response);
        }

        [HttpPost("netWorth")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(BadRequestDto))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(UnauthorizedDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundDto))]
        public async Task<ActionResult<List<StatisticItemDto>>> GetMyNetWorthStatistic(GetStatisticRequest request)
        {
            var validator = FieldValidationService.Create(request);

            validator
                .FieldIsRequired(x => x.Year)
                .FieldIsRequired(x => x.CurrencyId);

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
                return BadRequest(new BadRequestDto { Message = "Invalid request", Errors = [$"Currency with ID {request.CurrencyId} was not found"] });
            }

            var selectedCurrency = (Currencies)request.CurrencyId!;
            var expenseQuery = dbContext.Expenses.AsQueryable().Where(expense => expense.Date.Year == request.Year && expense.UserId == myId);
            var incomeQuery = dbContext.Incomes.AsQueryable().Where(income => income.Date.Year == request.Year && income.UserId == myId);
            List<StatisticItemDto> incomeStatistic;
            List<StatisticItemDto> expenseStatistic;

            if (selectedCurrency == Currencies.USD)
            {
                incomeStatistic = await incomeQuery.ToUSDStatisticDataAsync();
                expenseStatistic = await expenseQuery.ToUSDStatisticDataAsync();
            }
            else
            {
                incomeStatistic = await incomeQuery.ToEURStatisticDataAsync();
                expenseStatistic = await expenseQuery.ToEURStatisticDataAsync();
            }

            foreach (var monthIncome in incomeStatistic)
            {
                var monthExpense = expenseStatistic.Find(item => item.Month == monthIncome.Month);
                monthIncome.TotalAmount -= monthExpense!.TotalAmount;
            }

            return Ok(incomeStatistic);
        }

        [HttpPost("incomeDistribution")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(BadRequestDto))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(UnauthorizedDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundDto))]
        public async Task<ActionResult<List<DistributionItemDto>>> GetMyIncomeDistribution(GetDistributionRequest request)
        {
            var validator = FieldValidationService.Create(request);

            validator
                .FieldIsRequired(x => x.Year)
                .FieldIsRequired(x => x.Month);

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

            var response = await dbContext.Incomes.AsQueryable().Where(income => income.Date.Year == request.Year && income.Date.Month == request.Month && income.UserId == myId).ToDistributionDataAsync();

            return Ok(response);
        }

        [HttpPost("expenseDistribution")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(BadRequestDto))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(UnauthorizedDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundDto))]
        public async Task<ActionResult<List<DistributionItemDto>>> GetMyExpenseDistribution(GetDistributionRequest request)
        {
            var validator = FieldValidationService.Create(request);

            validator
                .FieldIsRequired(x => x.Year)
                .FieldIsRequired(x => x.Month);

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

            var response = await dbContext.Expenses.AsQueryable().Where(expense => expense.Date.Year == request.Year && expense.Date.Month == request.Month && expense.UserId == myId).ToDistributionDataAsync();

            return Ok(response);
        }
    }
}
