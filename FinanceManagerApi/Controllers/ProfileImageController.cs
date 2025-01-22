using FinanceManagerApi.Data;
using FinanceManagerApi.Extensions;
using FinanceManagerApi.Models.ProfileImage;
using Microsoft.AspNetCore.Mvc;

namespace FinanceManagerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileImageController(FinanceManagerDbContext dbContext) : ControllerBase
    {
        [HttpGet("getList")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<ProfileImageDto>>> GetAllProfileImages()
        {
            var response = await dbContext.ProfileImages.ToProfileImageDtoListAsync();

            return Ok(response);
        }
    }
}
