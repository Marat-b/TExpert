using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TExp.Models;

namespace TExp.Controllers
{
    public class UserListController : ApiController
    {
        private readonly TExpEntities _db;

        public UserListController()
        {
            _db=new TExpEntities();
        }

        [HttpGet]
        public IQueryable<AccountViewModel> Get()
        {
            IQueryable<AccountViewModel> accountViewModel = _db.t_User.Select(s => new AccountViewModel() { Id = s.UserID, UserName = s.UserName, FIO = s.FIO,Position = s.Position}).AsQueryable();
            return accountViewModel;
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
