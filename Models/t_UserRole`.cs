using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;

namespace TExp.Models
{
    public partial class t_UserRole: IRole<int>
    {
        public int Id {get { return RoleID; }}
        //public string Name { get; set; }
    }
}