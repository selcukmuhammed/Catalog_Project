using System.ComponentModel.DataAnnotations;

namespace TestDemo.Dtos
{
	public class RegisterDto
	{
		[Required(ErrorMessage = "Adı gerekli!")]
		public string FirstName { get; set; }

		[Required(ErrorMessage = "Soyadı gerekli!")]
		public string LastName { get; set; }

		[Required(ErrorMessage = "Kullanıcı adı gerekli!")]
		public string UserName { get; set; }

		[Required(ErrorMessage = "Email gerekli!")]
		public string Email { get; set; }

		[Required(ErrorMessage = "Şifre gerekli!")]
		public string Password { get; set; }
	}
}
