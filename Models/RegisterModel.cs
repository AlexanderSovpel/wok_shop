using System;
using System.ComponentModel.DataAnnotations;

namespace WokShop.Models
{
    public class RegisterModel
    {
        [Required]
        public string Email { get; set; }

        public string Role { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        [DataType(DataType.Password)]
        public string PasswordConfirm { get; set; }
    }
}