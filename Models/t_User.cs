//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TExp.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class t_User
    {
        public t_User()
        {
            this.t_UserRole = new HashSet<t_UserRole>();
            this.t_Expertise = new HashSet<t_Expertise>();
            this.t_UsedPassword = new HashSet<t_UsedPassword>();
        }
    
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string FIO { get; set; }
        public string PasswordHash { get; set; }
        public byte[] Sign { get; set; }
        public string Position { get; set; }
        public Nullable<System.DateTimeOffset> SetPasswordEndDate { get; set; }
        public int AccessFailedCount { get; set; }
        public bool LockoutEnabled { get; set; }
        public Nullable<System.DateTimeOffset> LockoutEndDate { get; set; }
        public bool SetPasswordEnabled { get; set; }
        public Nullable<System.DateTimeOffset> SigninEndDate { get; set; }
    
        public virtual ICollection<t_UserRole> t_UserRole { get; set; }
        public virtual ICollection<t_Expertise> t_Expertise { get; set; }
        public virtual ICollection<t_UsedPassword> t_UsedPassword { get; set; }
    }
}
