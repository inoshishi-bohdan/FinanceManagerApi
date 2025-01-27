using FinanceManagerApi.Entities;
using FinanceManagerApi.Models.Auth;
using FinanceManagerApi.Models.User;
using FinanceManagerApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace FinanceManagerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        public static User user = new();

        
        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UserDto>> Register(RegisterRequestDto request)
        {
            var validator = FieldValidator.Create(request);

            validator
                .FieldIsRequired(x => x.UserName).FieldHasMaxLength(x => x.UserName, 100)
                .FieldIsRequired(x => x.Email).FieldHasMaxLength(x => x.Email, 100).FieldHasValidEmailFormat(x => x.Email)
                .FieldIsRequired(x => x.Password);

            //check if request parameters is not null or missing
            if (validator.Any()) return validator.BadRequest();

            var user = await authService.RegisterAsync(request);

            if (user == null)
            {
                return BadRequest("Email already exists.");
            }

            return Ok(user);
        }

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<TokenResponseDto>> Login(LoginRequestDto request)
        {
            var validator = FieldValidator.Create(request);

            validator
                .FieldIsRequired(x => x.Email).FieldHasValidEmailFormat(x => x.Email)
                .FieldIsRequired(x => x.Password);

            //check if request parameters is not null or missing
            if (validator.Any()) return validator.BadRequest();

            var response = await authService.LoginAsync(request);

            if (response == null) 
            {
                return BadRequest("Invalid email or password.");
            }

            return Ok(response);
        }

        [HttpPost("refreshToken")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<TokenResponseDto>> RefreshToken(RefreshTokenRequestDto request)
        {
            var validator = FieldValidator.Create(request);

            validator
                .FieldIsRequired(x => x.UserId)
                .FieldIsRequired(x => x.RefreshToken);

            //check if request parameters is not null or missing
            if (validator.Any()) return validator.BadRequest();

            var response = await authService.RefreshTokensAsync(request);

            if (response == null || response.AccessToken == null || response.RefreshToken == null) 
            {
                return Unauthorized("Invalid refresh token.");
            }

            return Ok(response);
        }
    }
}
