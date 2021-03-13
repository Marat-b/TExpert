using System;
using System.Configuration;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using TExp.App_Start;
using TExp.Models;
using System.Diagnostics;
using System.Web.Http;
//using TExp.Providers;
//using TExp.Security;

[assembly: OwinStartup(typeof(TExp.Startup))]

namespace TExp
{
    public class Startup
    {
        

        public static string PublicClientId { get; private set; }

        public Startup()
        {
            
            
        }

        public void Configuration(IAppBuilder app)
        {
#if DEBUG
            Debug.WriteLine("Startup Configuration");
#endif
            app.CreatePerOwinContext(MyDbContext.Create);
            app.CreatePerOwinContext<MyUserManager>(MyUserManager.Create);
            app.CreatePerOwinContext<MySignInManager>(MySignInManager.Create);

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                ReturnUrlParameter = "returnUrl",
                ExpireTimeSpan = TimeSpan.FromDays(Convert.ToInt32(ConfigurationManager.AppSettings["ExpireTimeAuth"]))
                
                /*Provider = new CookieAuthenticationProvider
                {
                    // Enables the application to validate the security stamp when the user logs in.
                    // This is a security feature which is used when you change a password or add an external login to your account.  
                    OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<MyUserManager, t_User>(
                        validateInterval: TimeSpan.FromMinutes(20),
                        regenerateIdentity: (manager, user) => manager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie))
                }
                */

            }
                );

           


        }

      
    }
}
