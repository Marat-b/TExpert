using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

//using System.ComponentModel.DataAnnotations;


namespace TExp.Models
{
    public class AccountViewModel
    {
        public int Id {get; set;}
        public string UserName { get; set; }
        public string UserRole { get; set; }
        [Display(Name = "Ф. И. О.")]
        public string FIO { get; set; }
        [Display(Name = "Должность")]
        public string Position { get; set; }

        [Display(Name = "Смена пароля")]
        public bool SetPasswordEnabled { get; set; }

        [Display(Name = "Блокировка логина")]
        public bool LockoutEnabled { get; set; }

    }

    

}