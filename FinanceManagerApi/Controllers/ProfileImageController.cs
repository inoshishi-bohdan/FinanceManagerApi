using AutoMapper;
using FinanceManagerApi.Data;
using FinanceManagerApi.Models.ProfileImage;
using FinanceManagerApi.Models.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinanceManagerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileImageController(FinanceManagerDbContext dbContext, IMapper mapper) : ControllerBase
    {
        [HttpGet("getList")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<ProfileImageDto>>> GetAllProfileImages()
        {
            var profileImages = await dbContext.ProfileImages.ToListAsync();
            var response = mapper.Map<List<ProfileImageDto>>(profileImages);

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

            var response = mapper.Map<ProfileImageDto>(profileImage);

            return Ok(response);
        }
    }
}
