using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using TestDemo.Dtos;
using TestDemo.Interfaces;
using TestDemo.Models.Other_Objects;

namespace TestDemo.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthenticationController : ControllerBase
	{
		private readonly IAuthService _authService;

		public AuthenticationController(IAuthService authService)
		{
			_authService = authService;
		}

		[HttpPost("seed-roles")]
		public async Task<IActionResult> SeedRoles()
		{
			var seedRole = await _authService.SeedRolesAsync();
			return Ok(seedRole);
		}

		[HttpPost("register")]
		public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
		{
			var registerResult = await _authService.RegisterAsync(registerDto);

			if (registerResult.IsSucceed)
				return Ok(registerResult);

			return BadRequest(registerResult);
		}

		[HttpPost("login")]
		public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
		{
			var loginResult = await _authService.LoginAsync(loginDto);

			if (loginResult.IsSucceed)
				return Ok(loginResult);

			return Unauthorized(loginResult);
		}

		[HttpPost("make-admin")]
		public async Task<IActionResult> MakeAdmin([FromBody] UpdatePermissionDto updatePermissionDto)
		{
			var adminResult = await _authService.MakeAdminAsync(updatePermissionDto);

			if (adminResult.IsSucceed)
				return Ok(adminResult);

			return BadRequest(adminResult);
		}

		[HttpPost("make-owner")]
		public async Task<IActionResult> MakeOwner([FromBody] UpdatePermissionDto updatePermissionDto)
		{
			var ownerResult = await _authService.MakeOwnerAsync(updatePermissionDto);

			if (ownerResult.IsSucceed)
				return Ok(ownerResult);

			return BadRequest(ownerResult);
		}
	}
}
