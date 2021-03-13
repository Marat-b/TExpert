using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TExp.Models;

namespace TExp.Controllers
{
    public class ExpertiseListController : ApiController
    {
        private readonly TExpEntities _db;

        public ExpertiseListController()
        {
            _db=new TExpEntities();
        }

        [HttpGet]
        public IQueryable<ExpertiseModel> Get(int id)
        {
            IQueryable<ExpertiseModel> expertiseModels =
                _db.t_Equipment.Where(w => w.EquipmentId == id).FirstOrDefault().t_Expertise.Select(
                    s => new ExpertiseModel()
                    {
                        NumberExp = s.NumberExp,
                        ExpertiseId = s.ExpertiseId,
                        RequestId = s.RequestId,
                        DateExp = s.DateExp
                    }).AsQueryable();

            return expertiseModels;
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
