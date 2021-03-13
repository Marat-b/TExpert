using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using TExp.Models;

namespace TExp.Controllers
{
    public class DecommissionController : ApiController
    {
        private readonly TExpEntities db;

        public DecommissionController()
        {
            db = new TExpEntities();
        }

        [Route("api/Decommission/{id}")]
        [ResponseType(typeof(DecommissionModel))]
        public IHttpActionResult Get(int id)
        {
            DecommissionModel decommission = new DecommissionModel();
            t_Equipment equipment = db.t_Equipment.Find(id);
            if (equipment==null)
            {
                return NotFound();
            }

            decommission.StateOfDecommission = equipment.Decommission;
            decommission.DateOfDecommission = equipment.DateOfDecommission;

            return Ok(decommission);
        }

        [Route("api/Decommission/{id}")]
        public IHttpActionResult Put(int id,DecommissionModel decommission)
        {
            
            t_Equipment equipment = db.t_Equipment.Find(id);
            if (equipment == null)
            {
                return NotFound();
            }

            equipment.DateOfDecommission = decommission.DateOfDecommission;
            equipment.Decommission = decommission.StateOfDecommission;
            db.Entry(equipment).State = EntityState.Modified;
            try
            {
                db.SaveChanges();
            }
            catch (DBConcurrencyException)
            {
#if DEBUG
                Debug.WriteLine("Catch is occuried!");
#endif
                return NotFound();
            }
            return StatusCode(HttpStatusCode.NoContent);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}
