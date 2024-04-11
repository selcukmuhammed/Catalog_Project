using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TestDemo.Dtos;
using TestDemo.Interfaces;
using TestDemo.Models.Other_Objects;

namespace TestDemo.Services
{
	public class AuthService : IAuthService
	{
		private readonly UserManager<AppUser> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly IConfiguration _configuration;
		private readonly SignInManager<AppUser> _signInManager;

		public AuthService(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, SignInManager<AppUser> signInManager)
		{
			_userManager = userManager;
			_roleManager = roleManager;
			_configuration = configuration;
			_signInManager = signInManager;
		}

		public async Task<string> LoginAsync(LoginDto loginDto)
		{
		
			var user = await _userManager.FindByNameAsync(loginDto.UserName);

			if (user is null)
			{
				AuthServiceResponseDto authServiceResponseDto = new AuthServiceResponseDto()
				{
					IsSucceed = false,
					Message = "Geçersiz Kullanıcı adı!"
				};
				var result= JsonConvert.SerializeObject(authServiceResponseDto);
				return result;
			}
				

			var isPasswordCorrect = await _userManager.CheckPasswordAsync(user, loginDto.Password);

			if (!isPasswordCorrect)
			{
				AuthServiceResponseDto authServiceResponseDto = new AuthServiceResponseDto()
				{
					IsSucceed = false,
					Message = "Geçersiz Şifre!"
				};
				var result = JsonConvert.SerializeObject(authServiceResponseDto);
				return result;
			}
				

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

			return token;
		}

		public async Task<AuthServiceResponseDto> MakeAdminAsync(UpdatePermissionDto updatePermissionDto)
		{
			var user = await _userManager.FindByNameAsync(updatePermissionDto.UserName);

			if (user is null)
				return new AuthServiceResponseDto()
				{
					IsSucceed = false,
					Message = "Geçersiz Kullanıcı adı!"
				};

			await _userManager.AddToRoleAsync(user, UserRoles.ADMIN);
			return new AuthServiceResponseDto()
			{
				IsSucceed = true,
				Message = "Kullanıcı artık bir Admin!"
			};
		}

		public async Task<AuthServiceResponseDto> MakeOwnerAsync(UpdatePermissionDto updatePermissionDto)
		{
			var user = await _userManager.FindByNameAsync(updatePermissionDto.UserName);

			if (user is null)
				return new AuthServiceResponseDto()
				{
					IsSucceed = false,
					Message = "Geçersiz Kullanıcı adı!"
				};

			await _userManager.AddToRoleAsync(user, UserRoles.OWNER);
			return new AuthServiceResponseDto()
			{
				IsSucceed = true,
				Message = "Kullanıcı artık bir Owner!"
			};
		}

		public async Task<AuthServiceResponseDto> RegisterAsync(RegisterDto registerDto)
		{
			var isExistsUser = await _userManager.FindByIdAsync(registerDto.UserName);

			if (isExistsUser != null)
				return new AuthServiceResponseDto()
				{
					IsSucceed = false,
					Message = "Kullanıcı adı zaten var!"
				};

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
				return new AuthServiceResponseDto()
				{
					IsSucceed = false,
					Message = errorString
				};
			}

			await _userManager.AddToRoleAsync(newUser, UserRoles.USER);
			return new AuthServiceResponseDto()
			{
				IsSucceed = true,
				Message = "Kullanıcı başarıyla eklendi!"
			};
		}

		public async Task<AuthServiceResponseDto> SeedRolesAsync()
		{
			bool isAdminRoleExists = await _roleManager.RoleExistsAsync(UserRoles.ADMIN);
			bool isUserRoleExists = await _roleManager.RoleExistsAsync(UserRoles.USER);
			bool isOwnerRoleExists = await _roleManager.RoleExistsAsync(UserRoles.OWNER);

			if (isAdminRoleExists && isUserRoleExists && isOwnerRoleExists)
				return new AuthServiceResponseDto()
				{
					IsSucceed = true,
					Message = "Rol verme işlemi zaten yapıldı!"
				};

			await _roleManager.CreateAsync(new IdentityRole(UserRoles.ADMIN));
			await _roleManager.CreateAsync(new IdentityRole(UserRoles.USER));
			await _roleManager.CreateAsync(new IdentityRole(UserRoles.OWNER));

			return new AuthServiceResponseDto()
			{
				IsSucceed = true,
				Message = "Rol verme işlemi zaten yapıldı!"
			};
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
	}
}
