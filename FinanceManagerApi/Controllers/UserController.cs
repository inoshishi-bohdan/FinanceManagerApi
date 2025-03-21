using FinanceManagerApi.Data;
using FinanceManagerApi.Entities;
using FinanceManagerApi.Extensions;
using FinanceManagerApi.Models.Response;
using FinanceManagerApi.Models.User;
using FinanceManagerApi.Services.FieldValidationService;
using FinanceManagerApi.Services.UserService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinanceManagerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController(FinanceManagerDbContext dbContext, IUserService userService) : ControllerBase
    {
        [HttpGet("getMyInfo")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(UnauthorizedDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundDto))]
        public async Task<ActionResult<UserDto>> GetMyInfo()
        {
            var myId = userService.GetMyId();

            if (myId == null)
            {
                return Unauthorized(new UnauthorizedDto { Message = "Couldn't get user id from http context" });
            }

            var user = await dbContext.Users.AsQueryable().FirstOrDefaultAsync(x => x.Id == myId);

            //check if user record exists
            if (user == null)
            {
                return NotFound(new NotFoundDto { Message = $"User with ID {myId} was not found" });
            }

            var response = user.ToUserDto();
            
            return Ok(response);
        }

        [HttpPut("updateMyInfo")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(BadRequestDto))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(UnauthorizedDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundDto))]
        public async Task<ActionResult<UserDto>> UpdateMyInfo(UpdateUserRequest request)
        {
            var validator = FieldValidationService.Create(request);

            validator
                .FieldIsRequired(x => x.UserName).FieldHasMaxLength(x => x.UserName, 100)
                .FieldIsRequired(x => x.Email).FieldHasMaxLength(x => x.Email, 100).FieldHasValidEmailFormat(x => x.Email)
                .FieldIsRequired(x => x.OldPassword)
                .FieldIsRequired(x => x.NewPassword)
                .FieldIsRequired(x => x.ProfileImageId);

            //check if request parameters is not null or missing
            if (validator.Any()) return validator.BadRequest();

            var myId = userService.GetMyId();

            if (myId == null)
            {
                return Unauthorized(new UnauthorizedDto { Message = "Couldn't get user id from http context" });
            }

            var user = await dbContext.Users.AsQueryable().FirstOrDefaultAsync(x => x.Id == myId);

            //check if user record exists
            if (user == null)
            {
                return NotFound(new NotFoundDto { Message = $"User with ID {myId} was not found" });
            }

            if (new PasswordHasher<User>().VerifyHashedPassword(user, user.PasswordHash, request.OldPassword!) == PasswordVerificationResult.Failed)
            {
                return BadRequest(new BadRequestDto { Message = "Invalid request", Errors = new List<string> { $"Invalid password" } });
            }

            if (!dbContext.ProfileImages.Any(profileImage => profileImage.Id == request.ProfileImageId))
            {
                return BadRequest(new BadRequestDto { Message = "Invalid request", Errors = new List<string> { $"Profile image with ID {request.ProfileImageId} was not found" } });
            }

            user.UserName = request.UserName!;
            user.Email = request.Email!;
            user.PasswordHash = new PasswordHasher<User>().HashPassword(user, request.NewPassword!);
            user.ProfileImageId = (int)request.ProfileImageId!;
            user.RefreshToken = null;
            user.RefreshTokenExpiryTime = null;

            await dbContext.SaveChangesAsync();

            return Ok(user.ToUserDto());
        }

        [HttpDelete("deleteMyAccount")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(UnauthorizedDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundDto))]
        public async Task<ActionResult<string>> DeleteMyAccount()
        {
            var myId = userService.GetMyId();

            if (myId == null)
            {
                return Unauthorized(new UnauthorizedDto { Message = "Couldn't get user id from http context" });
            }

            var user = await dbContext.Users.AsQueryable().FirstOrDefaultAsync(x => x.Id == myId);

            //check if user record exists
            if (user == null)
            {
                return NotFound(new NotFoundDto { Message = $"User with ID {myId} was not found" });
            }

            dbContext.Users.Remove(user);

            await dbContext.SaveChangesAsync();

            return Ok($"User with ID {user.Id} was successfully deleted.");
        }
    }
}
