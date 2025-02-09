using FinanceManagerApi.Data;
using FinanceManagerApi.Extensions;
using FinanceManagerApi.Models.ProfileImage;
using FinanceManagerApi.Models.Response;
using FinanceManagerApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinanceManagerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileImageController(FinanceManagerDbContext dbContext, IUserService userService) : ControllerBase
    {
        [HttpGet("getList")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<ProfileImageDto>>> GetAllProfileImages()
        {
            var response = await dbContext.ProfileImages.ToProfileImageDtoListAsync();

            return Ok(response);
        }

        [HttpGet("getMyProfileImage")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(UnauthorizedDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundDto))]
        [Authorize]
        public async Task<ActionResult<ProfileImageDto>> GetMyProfileImage()
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

            var profileImage = await dbContext.ProfileImages.FindAsync(user.ProfileImageId);

            if (profileImage == null)
            {
                return NotFound(new NotFoundDto { Message = $"Profile image with ID {user.ProfileImageId} was not found" });
            }

            return Ok(profileImage.ToProfileImageDto());
        }
    }
}
