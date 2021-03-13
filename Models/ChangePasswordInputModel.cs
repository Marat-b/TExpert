using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TExp.Models
{
    public class ChangePasswordInputModel:ResetPasswordInputModel
    {
        
       [Display(Name = "Старый пароль")]
       [Required(AllowEmptyStrings = false, ErrorMessage = "Поле ввода требует заполнения")]
       public string OldPassword { get; set; } 
    }
}