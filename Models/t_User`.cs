using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
//using TestIdentityCS.DB;

namespace TExp.Models
{
    public partial class t_User:IUser<int>
    {
        public int Id { get { return UserID; } }
        //string IUser<string>.Id { get { return Id.ToString(); } }
        // public string UserName { get; set; }
        //public int AccessFailedCount { get; set; }
       // public bool LockoutEnabled { get; set; }
        //public DateTimeOffset? LockoutEndDateUtc { get; set; }
    }
}