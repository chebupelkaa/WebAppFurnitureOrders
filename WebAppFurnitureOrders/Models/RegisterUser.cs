using AutoMapper.Configuration.Annotations;
using System.ComponentModel.DataAnnotations;

namespace WebAppFurnitureOrders.Models
{
    public class RegisterUser
    {
        [Required]
        [EmailAddress(ErrorMessage = "Неправильный формат Email")]
        [Display(Name = "Email")]
        public string Email { get; set; } = String.Empty;

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; } = String.Empty;

        [Required]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        [DataType(DataType.Password)]
        [Display(Name = "Подтвердить пароль")]
        public string PasswordConfirm { get; set; } = String.Empty;
    }
}
