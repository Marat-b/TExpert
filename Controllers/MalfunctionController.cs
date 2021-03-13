using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using TExp.Models;

namespace TExp.Controllers
{
    public class MalfunctionController : ApiController
    {
        private TExpEntities db = new TExpEntities();

        // GET: api/Malfunction
        public IQueryable<MalfunctionModel> Gett_Malfunction()
        {
            return db.t_Malfunction.Select(s => new MalfunctionModel() {FaultId=s.FaultId,Name=s.Name });
        }

        // GET: api/Malfunction/5
        [ResponseType(typeof(MalfunctionModel))]
        public IHttpActionResult Gett_Malfunction(int id)
        {
            MalfunctionModel t_Malfunction =  db.t_Malfunction.Where(w=>w.FaultId==id).Select(s => new MalfunctionModel() {FaultId=s.FaultId,Name=s.Name }).FirstOrDefault();
            if (t_Malfunction == null)
            {
                return NotFound();
            }

            return Ok(t_Malfunction);
        }

        // PUT: api/Malfunction/5
        [ResponseType(typeof(void))]
        public IQueryable<MalfunctionModel> Putt_Malfunction(int id, MalfunctionModel malfunctionModel)
        {
            if (!ModelState.IsValid)
            {
                //return BadRequest(ModelState);
                return db.t_Malfunction.Select(s => new MalfunctionModel() { FaultId = s.FaultId, Name = s.Name });
            }

            t_Malfunction malfunction = db.t_Malfunction.Find(id);

            if (malfunction==null)
            {
                //return BadRequest();
                return db.t_Malfunction.Select(s => new MalfunctionModel() { FaultId = s.FaultId, Name = s.Name });
            }

            malfunction.Name = malfunctionModel.Name;

            try
            {
                db.Entry(malfunction).State = EntityState.Modified;
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!t_MalfunctionExists(id))
                {
                    //return NotFound();
                    return db.t_Malfunction.Select(s => new MalfunctionModel() { FaultId = s.FaultId, Name = s.Name }); 
                }
                else
                {
                    throw;
                }
            }

            //return StatusCode(HttpStatusCode.NoContent);
            return db.t_Malfunction.Select(s => new MalfunctionModel() { FaultId = s.FaultId, Name = s.Name }); 
        }

        // POST: api/Malfunction
        [ResponseType(typeof(t_Malfunction))]
        public async Task<IQueryable<MalfunctionModel>> Postt_Malfunction(t_Malfunction t_Malfunction)
        {
            if (!ModelState.IsValid)
            {
                //return BadRequest(ModelState);
                return db.t_Malfunction.Select(s => new MalfunctionModel() { FaultId = s.FaultId, Name = s.Name });
            }

            db.t_Malfunction.Add(t_Malfunction);
            await db.SaveChangesAsync();
            return db.t_Malfunction.Select(s => new MalfunctionModel() { FaultId = s.FaultId, Name = s.Name });
            //return CreatedAtRoute("DefaultApi", new { id = t_Malfunction.FaultId }, t_Malfunction);
        }

        // DELETE: api/Malfunction/5
        [ResponseType(typeof(MalfunctionModel))]
        public async Task<IQueryable<MalfunctionModel>> Deletet_Malfunction(int id)
        {
            t_Malfunction t_Malfunction =await db.t_Malfunction.FindAsync(id);
            if (t_Malfunction == null)
            {
                //return NotFound();
                return db.t_Malfunction.Select(s => new MalfunctionModel() { FaultId = s.FaultId, Name = s.Name });
            }

            db.t_Malfunction.Remove(t_Malfunction);
            await db.SaveChangesAsync();

            //return Ok(t_Malfunction);
            return db.t_Malfunction.Select(s => new MalfunctionModel() { FaultId = s.FaultId, Name = s.Name });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool t_MalfunctionExists(int id)
        {
            return db.t_Malfunction.Count(e => e.FaultId == id) > 0;
        }
    }
}