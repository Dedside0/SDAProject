using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SDA.Web.Models
{
    public class Registration
    {
        [Display(Name = "Имя пользователя", Prompt = "Ваше имя")]
        [Required(ErrorMessage = "Не указано имя")]
        [StringLength(25, MinimumLength = 2, ErrorMessage = "Длина должна быть от {2} до {1} символов")]
        public string Name { get; set; }


        [Display(Name = "Пароль", Prompt = "Введите пароль")]
        [Required(ErrorMessage = "Введите пароль")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Пароль должен быть не менее 8 символов")]
        [DataType(DataType.Password)]
        public string Password { get; set; }


        [Display(Name = "Повторите пароль", Prompt = "Повторите пароль")]
        [Required(ErrorMessage = "Не указан повторный пароль")]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }


        [Display(Name = "Email", Prompt = "example@mail.com")]
        [Required(ErrorMessage = "Не указан логин")]
        [EmailAddress(ErrorMessage = "Введите валидный email")]
        public string Login { get; set; }
    }
}
