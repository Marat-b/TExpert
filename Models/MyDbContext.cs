using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;

namespace TExp.Models
{
    public class MyDbContext:DbContext
    {

        public MyDbContext() : base("TExpEntities")
        {
           
        }

        public DbSet<t_User> User { get; set; }
        public DbSet<t_UserRole> UserRole { get; set; }
        public DbSet<t_UsedPassword> UsedPassword { get; set; }

        public static MyDbContext Create()
        {
            return new MyDbContext();
        }
    }
}