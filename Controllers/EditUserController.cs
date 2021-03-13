using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects.DataClasses;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using TExp.App_Start;
using TExp.Classes;
using TExp.Models;

namespace TExp.Controllers
{
    public class EditUserController : ApiController
    {
        //private readonly TExpEntities _db;
        private readonly MyUserManager _userManager;
        private string _userName;

        public EditUserController()
        {
            //_db = new TExpEntities();
            _userManager= System.Web.HttpContext.Current.GetOwinContext().GetUserManager<MyUserManager>();
            _userName = User.Identity.Name;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">UserId</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult Get(int id)
        {
            AccountViewModel accountViewModel =new AccountViewModel();
                /*_db.t_User.Where(w => w.UserID == id)
                    .Select(s => new AccountViewModel() {Id = s.UserID, UserName = s.UserName, FIO = s.FIO,Position = s.Position,SetPasswordEnabled = s.SetPasswordEnabled,LockoutEnabled = s.LockoutEnabled})
                    .FirstOrDefault();*/
            var user = _userManager.FindByIdAsync(id).Result;
            accountViewModel.Id = user.UserID;
            accountViewModel.UserName = user.UserName;
            accountViewModel.FIO = user.FIO;
            accountViewModel.Position = user.Position;
            accountViewModel.SetPasswordEnabled = user.SetPasswordEnabled;
            accountViewModel.LockoutEnabled = user.LockoutEnabled;
            string userRole= _userManager.GetRolesAsync(id).Result.FirstOrDefault();
            if (userRole != null)
            {
                accountViewModel.UserRole = userRole;
            }
            return Ok(accountViewModel);
        }

        [Route("api/EditUser/GetImage/{id}")]
        [HttpGet]
        public HttpResponseMessage GetImage(int id)
        {
            //byte[] userImage = _db.t_User.Where(w => w.UserID == id).Select(s=>s.Sign).FirstOrDefault();
            byte[] userImage = _userManager.FindByIdAsync(id).Result.Sign;
            HttpResponseMessage response;
            if (userImage != null)
            {

                //MediaTypeNames.Image img = (MediaTypeNames.Image)data.SingleOrDefault();
                //byte[] imgData = user.Sign;
                string strBase64 = Convert.ToBase64String(userImage);
                byte[] byteBase64 = Encoding.ASCII.GetBytes(strBase64);
                //MemoryStream ms = new MemoryStream(imgData);
                MemoryStream ms = new MemoryStream(byteBase64);
                response = new HttpResponseMessage(HttpStatusCode.OK);
                response.Content = new StreamContent(ms);
                //    response.Content = new ByteArrayContent(ms.ToArray());
                response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/jpeg");
                //response.Headers.
                return response;
            }
            else
            {
                response = new HttpResponseMessage(HttpStatusCode.NoContent);
                return response;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">User Id</param>
        /// <param name="accountView">AccountViewModel</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IHttpActionResult> Post(int id, AccountViewModel accountView)
        {
            //UserRoleRole userRoleRole = new UserRoleRole();
            if (id == 0)
            {
                /*
                t_User user=new t_User();
                user.LockoutEnabled = accountView.LockoutEnabled;
                user.Position = accountView.Position;
                user.SetPasswordEnabled = accountView.SetPasswordEnabled;
                user.FIO = accountView.FIO;
                user.UserName = accountView.UserName;
                _db.t_User.Add(user);
                _db.SaveChanges();
                _db.Entry(user).GetDatabaseValues();
                //int userId= _userManager.GetIdByName(_userName);
                await _userManager.AddUserToRoleAsync(user.UserID, accountView.UserRole);
                //userRoleRole.AddUserToRole(user.UserID, accountView.UserRole);*/
                //_userManager.UpdateAsync(user);
                if (await _userManager.CreateUserAsync(accountView) == IdentityResult.Success)
                {
                    return Ok();
                }
                return BadRequest();
            }
            else
            {
                //t_User user = _db.t_User.Find(id);
                var user=await _userManager.FindByIdAsync(id);
                if (user != null)
                {
                    user.LockoutEnabled = accountView.LockoutEnabled;
                    user.Position = accountView.Position;
                    user.SetPasswordEnabled = accountView.SetPasswordEnabled;
                    user.FIO = accountView.FIO;
                    user.UserName = accountView.UserName;
                    //_db.Entry(user).State=EntityState.Modified;
                    //_db.SaveChanges();
                    await _userManager.UpdateAsync(user);
                    //int userId = _userManager.GetIdByName(_userName);
                    await _userManager.RemoveUserFromRoleAsync(id);
                    //userRoleRole.RemoveUserFromRole(id, accountView.UserRole);

                    await _userManager.AddUserToRoleAsync(id, accountView.UserRole);
                    //userRoleRole.AddUserToRole(id, accountView.UserRole);
                    //_userManager.UpdateAsync(user);
                    return Ok();
                }
                else
                {
                    return BadRequest();
                }
            }

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                //_db.Dispose();
                _userManager.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}
