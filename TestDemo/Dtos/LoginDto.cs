using System.ComponentModel.DataAnnotations;

namespace TestDemo.Dtos
{
	public class LoginDto
	{
        [Required(ErrorMessage = "Kullanıcı adı gerekli!")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Şifre gerekli!")]
        public string Password { get; set; }
    }
}
