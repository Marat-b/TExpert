using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;

namespace TExp.Models
{
    public class MyUser:IUser<int>,IUser
    {
        public int Id { get {return UserID;} }
        string IUser<string>.Id { get { return Id.ToString(); } }
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string FIO { get; set; }
        public string PasswordHash { get; set; }
        public int AccessFailedCount { get; set; }
        public bool LockoutEnabled { get; set; }
        public DateTimeOffset? LockoutEndDateUtc { get; set; }
    }
}