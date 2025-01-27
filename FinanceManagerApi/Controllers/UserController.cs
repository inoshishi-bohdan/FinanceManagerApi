using Azure.Core;
using FinanceManagerApi.Data;
using FinanceManagerApi.Entities;
using FinanceManagerApi.Extensions;
using FinanceManagerApi.Models.User;
using FinanceManagerApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinanceManagerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController(FinanceManagerDbContext dbContext, IUserService userService, IAuthService authService) : ControllerBase
    {
        [HttpPut("updateMyInfo")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<UserDto>> UpdateMyInfo(UpdateUserRequestDto request)
        {
            var validator = FieldValidator.Create(request);

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
                return Unauthorized("Couldn't get user id from http context.");
            }

            var user = await dbContext.Users.AsQueryable().FirstOrDefaultAsync(x => x.Id == myId);

            //check if user record exists
            if (user == null)
            {
                return NotFound($"User with ID {myId} was not found.");
            }

            if (new PasswordHasher<User>().VerifyHashedPassword(user, user.PasswordHash, request.OldPassword!) == PasswordVerificationResult.Failed)
            {
                return BadRequest($"Invalid password.");
            }

            if (!dbContext.ProfileImages.Any(profileImage => profileImage.Id == request.ProfileImageId))
            {
                return BadRequest($"Profile image with ID {request.ProfileImageId} was not found.");
            }

            user.UserName = request.UserName!;
            user.Email = request.Email!;
            user.PasswordHash = new PasswordHasher<User>().HashPassword(user, request.NewPassword!);
            user.ProfileImageId = request.ProfileImageId;

            await dbContext.SaveChangesAsync();

            return Ok(user.ToUserDto());
        }
    }
}
