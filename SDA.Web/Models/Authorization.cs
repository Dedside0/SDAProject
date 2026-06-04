using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SDA.Web.Models
{
    public class Authorization
    {

        [Display(Name = "Пароль", Prompt = "Введите пароль")]
        [Required(ErrorMessage = "Введите пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Логин", Prompt = "example@mail.com")]
        [Required(ErrorMessage = "Не указан логин")]
        [EmailAddress(ErrorMessage = "Введите валидный email")]
        public string Login { get; set; }
        public bool IsRememberMe { get; set; }
    }
}
