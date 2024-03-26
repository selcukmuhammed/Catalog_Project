using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using TestDemo.Dtos;
using TestDemo.Models.Other_Objects;

namespace TestDemo.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthenticationController : ControllerBase
	{
		private readonly UserManager<AppUser> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly IConfiguration _configuration;

		public AuthenticationController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
		{
			_userManager = userManager;
			_roleManager = roleManager;
			_configuration = configuration;
		}

		[HttpPost("seed-roles")]
		public async Task<IActionResult> SeedRoles()
		{
			bool isAdminRoleExists = await _roleManager.RoleExistsAsync(UserRoles.ADMIN);
			bool isUserRoleExists = await _roleManager.RoleExistsAsync(UserRoles.USER);
			bool isOwnerRoleExists = await _roleManager.RoleExistsAsync(UserRoles.OWNER);

			if (isAdminRoleExists && isUserRoleExists && isOwnerRoleExists)
				return Ok("Rol verme işlemi zaten yapıldı!");

			await _roleManager.CreateAsync(new IdentityRole(UserRoles.ADMIN));
			await _roleManager.CreateAsync(new IdentityRole(UserRoles.USER));
			await _roleManager.CreateAsync(new IdentityRole(UserRoles.OWNER));

			return Ok("Rol işlemi tamamlandı!");
		}

		[HttpPost("register")]
		public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
		{
			var isExistsUser = await _userManager.FindByIdAsync(registerDto.UserName);

			if (isExistsUser != null)
				return BadRequest("Kullanıcı adı zaten var!");

			AppUser newUser = new AppUser()
			{
				FirstName = registerDto.FirstName,
				LastName = registerDto.LastName,
				UserName = registerDto.UserName,
				Email = registerDto.Email,
				SecurityStamp = Guid.NewGuid().ToString(),
			};

			var createUserResult = await _userManager.CreateAsync(newUser, registerDto.Password);

			if (!createUserResult.Succeeded)
			{
				var errorString = "Kullanıcı oluşturma başarısız oldu çünkü : ";
				foreach (var error in createUserResult.Errors)
				{
					errorString += " # " + error.Description;
				}
				return BadRequest(errorString);
			}

			await _userManager.AddToRoleAsync(newUser, UserRoles.USER);
			return Ok("Kullanıcı başarıyla eklendi!");
		}

		[HttpPost("login")]
		public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
		{
			var user = await _userManager.FindByNameAsync(loginDto.UserName);

			if (user is null)
				return Unauthorized("Geçersiz Kullanıcı adı!");

			var isPasswordCorrect = await _userManager.CheckPasswordAsync(user, loginDto.Password);

			if (!isPasswordCorrect)
				return Unauthorized("Geçersiz Şifre!");

			var userRoles = await _userManager.GetRolesAsync(user);

			var authClaims = new List<Claim>
			{
				new Claim(ClaimTypes.Name,user.UserName),
				new Claim(ClaimTypes.NameIdentifier,user.Id),
				new Claim("JWTID",Guid.NewGuid().ToString()),
				new Claim("FirstName",user.FirstName),
				new Claim("LastName",user.LastName),
			};

			foreach (var userRole in userRoles)
			{
				authClaims.Add(new Claim(ClaimTypes.Role, userRole));
			}

			var token = GenerateNewJsonWebToken(authClaims);

			return Ok(token);
		}

		private string GenerateNewJsonWebToken(List<Claim> claims)
		{
			var authSecret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:KEY"]));

			var tokenObject = new JwtSecurityToken(
				issuer: _configuration["JWT:ValidIssuer"],
				audience: _configuration["JWT:ValidAudience"],
				expires: DateTime.Now.AddHours(1),
				claims: claims,
				signingCredentials: new SigningCredentials(authSecret, SecurityAlgorithms.HmacSha256));

			string token = new JwtSecurityTokenHandler().WriteToken(tokenObject);

			return token;
		}

		[HttpPost("make-admin")]
		public async Task<IActionResult> MakeAdmin([FromBody] UpdatePermissionDto updatePermissionDto)
		{
			var user = await _userManager.FindByNameAsync(updatePermissionDto.UserName);

			if (user is null)
				return BadRequest("Geçersiz Kullanıcı adı!");

			await _userManager.AddToRoleAsync(user, UserRoles.ADMIN);
			return Ok("Kullanıcı artık bir Admin!");
		}

		[HttpPost("make-owner")]
		public async Task<IActionResult> MakeOwner([FromBody] UpdatePermissionDto updatePermissionDto)
		{
			var user = await _userManager.FindByNameAsync(updatePermissionDto.UserName);

			if (user is null)
				return BadRequest("Geçersiz Kullanıcı adı!");

			await _userManager.AddToRoleAsync(user, UserRoles.OWNER);
			return Ok("Kullanıcı artık bir Owner!");
		}
	}
}
