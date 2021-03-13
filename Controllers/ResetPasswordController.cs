using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Microsoft.AspNet.Identity.Owin;
using TExp.App_Start;
using TExp.Models;

namespace TExp.Controllers
{
    public class ResetPasswordController : ApiController
    {
        private readonly TExpEntities _db;
        private MyUserManager _userManager;

        public ResetPasswordController()
        {
            _db=new TExpEntities();
            _userManager = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<MyUserManager>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">UserId </param>
        /// <param name="userPassword">UserPassword </param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IHttpActionResult> Post(int id, ResetPasswordSingleModal userPassword)
        {

            if (userPassword != null)
            {
                bool ret= await _userManager.SetPasswordAsync(id, userPassword.Password);
                if (ret)
                {
                    return Ok();
                }
                return BadRequest();
            }
            else
            {
                return BadRequest();
            }
            
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
