using FinanceManagerApi.Data;
using FinanceManagerApi.Extensions;
using FinanceManagerApi.Models.ProfileImage;
using FinanceManagerApi.Models.Response;
using FinanceManagerApi.Services.UserService;
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

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundDto))]
        public async Task<ActionResult<ProfileImageDto>> GetProfileImage(int id)
        {
            var profileImage = await dbContext.ProfileImages.FindAsync(id);

            if (profileImage == null)
            {
                return NotFound(new NotFoundDto { Message = $"Profile image with ID {id} was not found" });
            }

            return Ok(profileImage.ToProfileImageDto());
        }
    }
}
