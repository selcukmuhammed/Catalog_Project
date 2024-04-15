using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace TestDemo.Services
{
	public static class TokenValidator
	{
		public static bool IsTokenValid(string token)
		{
			var tokenHandler = new JwtSecurityTokenHandler();
			var validationParameters = new TokenValidationParameters
			{
				ValidateIssuerSigningKey = true,
				IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("951CC7D8-C2A7-42D4-8D99-3DA0345EFBA9")),
				ValidateIssuer = false,
				ValidateAudience = false
			};

			try
			{
				SecurityToken validatedToken;
				var claimsPrincipal = tokenHandler.ValidateToken(token, validationParameters, out validatedToken);

				// Token içerisindeki "JWTID" claim'ine erişme
				var jwtIdClaim = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == "JWTID");

				// Eğer "JWTID" claim'i varsa ve değeri doğruysa true döndür
				if (jwtIdClaim != null)
				{
					return true;
				}
				else
				{
					return false;
				}
			}
			catch (Exception ex)
			{
				// Token doğrulama sırasında hata oluşursa
				return false;
			}
		}
	}
}
