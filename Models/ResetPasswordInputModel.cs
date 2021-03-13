using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace TExp.Models
{
    public class ResetPasswordInputModel
    {
        
        [Display(Name = "Имя пользователя")]
        public string UserName { get; set; }
        public int Id { get; set; }

       

        [Required(AllowEmptyStrings = false, ErrorMessage = "Поле ввода требует заполнения")]
        [StringLength(100, ErrorMessage =
        "{0} должен быть не менее {2} символов.", MinimumLength = 1)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        
        [Display(Name = "Подтвердить пароль")]
        [Compare("Password", ErrorMessage =
        "Пароль и подтверждение пароля не совпадают.")]
        public string ConfirmPassword { get; set; }
    }
}