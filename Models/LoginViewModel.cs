using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace TExp.Models
{
    public class LoginViewModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Поле ввода требует заполнения")]
        [Display(Name = "Имя пользователя")]
        public string UserName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Поле ввода требует заполнения")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }
    }
}