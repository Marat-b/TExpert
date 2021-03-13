using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
//using TExp.Models;

namespace TExp.Models
{
    public class RegisterInputModel
    {
        [Required]
        [Display(Name = "Имя пользователя")]
        public string UserName { get; set; }
        
        [Required]
        [StringLength(100, ErrorMessage =
        "{0} должен быть не менее {2} символов.", MinimumLength = 1)]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Подтвердить пароль")]
        [Compare("Password", ErrorMessage =
        "Пароль и подтверждение пароля не совпадают.")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Ф. И. О.")]
        public string FIO { get; set; }

        [Display(Name = "Должность")]
        public string Position { get; set; }

        [Display(Name = "Смена пароля")]
        public bool SetPasswordEnabled { get; set; }

        
        public IEnumerable<UserRoleViewModel> userRoles { get; set; }

    }
}