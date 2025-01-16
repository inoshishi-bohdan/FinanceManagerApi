﻿using FinanceManagerApi.Entities;
using FinanceManagerApi.Models;
using FinanceManagerApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace FinanceManagerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        public static User user = new();

        //check difference between ActionResult and IActionResult
        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(UserDto request)
        {
            var user = await authService.RegisterAsync(request);

            if (user == null)
            {
                return BadRequest("Username already exists.");
            }

            return Ok(user);
        }

        [HttpPost("login")]
        public async Task<ActionResult<TokenResponseDto>> Login(UserDto request)
        {
            var response = await authService.LoginAsync(request);

            if (response == null) 
            {
                return BadRequest("Invalid username or password.");
            }

            return Ok(response);
        }

        [HttpPost("refreshToken")]
        public async Task<ActionResult<TokenResponseDto>> RefreshToken(RefreshTokenRequestDto request)
        {
            var response = await authService.RefreshTokensAsync(request);

            if (response == null || response.AccessToken == null || response.RefreshToken == null) 
            {
                return Unauthorized("Invalid refresh token.");
            }

            return Ok(response);
        }
    }
}
