using System.ComponentModel.DataAnnotations;

namespace MVCatalog.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Kullanıcı adı gerekli!")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Şifre gerekli!")]
        public string Password { get; set; }
    }
}
