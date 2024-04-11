using TestDemo.Dtos;

namespace TestDemo.Interfaces
{
	public interface IAuthService
	{
		Task<AuthServiceResponseDto> SeedRolesAsync();
		Task<AuthServiceResponseDto> RegisterAsync(RegisterDto registerDto);
		Task<string> LoginAsync(LoginDto loginDto);
		Task<AuthServiceResponseDto> MakeAdminAsync(UpdatePermissionDto updatePermissionDto);
		Task<AuthServiceResponseDto> MakeOwnerAsync(UpdatePermissionDto updatePermissionDto);
	}
}
