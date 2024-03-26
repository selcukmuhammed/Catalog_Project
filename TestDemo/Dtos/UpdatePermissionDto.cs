using System.ComponentModel.DataAnnotations;

namespace TestDemo.Dtos
{
	public class UpdatePermissionDto
	{
        [Required(ErrorMessage = "Kullanıcı adı gerekli!")]
        public string UserName { get; set; }
    }
}
