using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using TExp.Models;
using System.Diagnostics;

namespace TExp.Controllers
{
    public class t_TypeOfEquipmentController : ApiController
    {
        private TExpEntities db; 

        public t_TypeOfEquipmentController()
        {
            db= new TExpEntities();
            //db.Configuration.ProxyCreationEnabled = false;

        }

        // GET: api/t_TypeOfEquipment
        public IQueryable<TypeofEquipmentModel> Gett_TypeOfEquipment()
        {
#if DEBUG
            foreach(t_TypeOfEquipment item in db.t_TypeOfEquipment.ToList())
            {
                Debug.WriteLine("TypeId={0},Name={1}",item.TypeId.ToString(),item.Name);
            }

            Debug.WriteLine(db.t_TypeOfEquipment.ToString());
#endif
            return db.t_TypeOfEquipment.Select(s => new TypeofEquipmentModel() { TypeId = s.TypeId, Name = s.Name, Effective = s.Effective, Warranty = s.Warranty, Limit = s.Limit }).AsQueryable();
        }

        // GET: api/t_TypeOfEquipment/5
        [ResponseType(typeof(TypeofEquipmentModel))]
        public IHttpActionResult Gett_TypeOfEquipment(int id)
        {
            TypeofEquipmentModel typeOfEquipmentModel = db.t_TypeOfEquipment.Where(w=>w.TypeId==id).Select(s => new TypeofEquipmentModel() { TypeId = s.TypeId, Name = s.Name, Effective = s.Effective, Warranty = s.Warranty, Limit = s.Limit }).FirstOrDefault();
            if (typeOfEquipmentModel == null)
            {
                return NotFound();
            }

            return Ok(typeOfEquipmentModel);
        }

        // PUT: api/t_TypeOfEquipment/5
        [ResponseType(typeof(void))]
        //public IHttpActionResult Putt_TypeOfEquipment(int id, t_TypeOfEquipment t_TypeOfEquipment)
        public IQueryable<TypeofEquipmentModel> Putt_TypeOfEquipment(int id, TypeofEquipmentModel typeOfEquipmentModel)
        {
            if (!ModelState.IsValid)
            {
                //return BadRequest(ModelState);
                return db.t_TypeOfEquipment.Select(s => new TypeofEquipmentModel() { TypeId = s.TypeId, Name = s.Name, Effective = s.Effective, Warranty = s.Warranty, Limit = s.Limit }).AsQueryable();
            }

            t_TypeOfEquipment typeOfEquipment = db.t_TypeOfEquipment.Find(id);

            if (typeOfEquipment == null)
            {
                //return BadRequest();
                return db.t_TypeOfEquipment.Select(s => new TypeofEquipmentModel() { TypeId = s.TypeId, Name = s.Name, Effective = s.Effective, Warranty = s.Warranty, Limit = s.Limit }).AsQueryable();
            }

            typeOfEquipment.Name = typeOfEquipmentModel.Name;
            typeOfEquipment.Warranty = typeOfEquipmentModel.Warranty;
            typeOfEquipment.Limit = typeOfEquipmentModel.Limit;
            typeOfEquipment.Effective = typeOfEquipmentModel.Effective;

            try
            {
                db.Entry(typeOfEquipment).State = EntityState.Modified;
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!t_TypeOfEquipmentExists(id))
                {
                    //return NotFound();
                    return db.t_TypeOfEquipment.Select(s => new TypeofEquipmentModel() { TypeId = s.TypeId, Name = s.Name, Effective = s.Effective, Warranty = s.Warranty, Limit = s.Limit }).AsQueryable();
                }
                else
                {
                    throw;
                }
            }
            return db.t_TypeOfEquipment.Select(s => new TypeofEquipmentModel() {TypeId=s.TypeId,Name=s.Name,Effective=s.Effective,Warranty=s.Warranty,Limit=s.Limit }).AsQueryable();
            //return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/t_TypeOfEquipment
        [ResponseType(typeof(TypeofEquipmentModel))]
        public IQueryable<TypeofEquipmentModel> Postt_TypeOfEquipment(t_TypeOfEquipment t_TypeOfEquipment)
        //public IHttpActionResult Postt_TypeOfEquipment(t_TypeOfEquipment t_TypeOfEquipment)
        {
            if (!ModelState.IsValid)
            {
#if DEBUG
                Debug.WriteLine("Model is NOT valid!");
#endif
                //return BadRequest(ModelState);
                return db.t_TypeOfEquipment.Select(s => new TypeofEquipmentModel() { TypeId = s.TypeId, Name = s.Name, Effective = s.Effective, Warranty = s.Warranty, Limit = s.Limit }).AsQueryable(); 
            }
#if DEBUG
            Debug.WriteLine("t_TypeOfEquipment.Name="+t_TypeOfEquipment.Name.ToString());
#endif
            db.t_TypeOfEquipment.Add(t_TypeOfEquipment);
            db.SaveChanges();

            return db.t_TypeOfEquipment.Select(s => new TypeofEquipmentModel() { TypeId = s.TypeId, Name = s.Name, Effective = s.Effective, Warranty = s.Warranty, Limit = s.Limit }).AsQueryable(); 
            //return CreatedAtRoute("DefaultApi", new { id = t_TypeOfEquipment.TypeId }, t_TypeOfEquipment);
        }

        // DELETE: api/t_TypeOfEquipment/5
        [ResponseType(typeof(TypeofEquipmentModel))]
        public IQueryable<TypeofEquipmentModel> Deletet_TypeOfEquipment(int id)
        {
            t_TypeOfEquipment t_TypeOfEquipment = db.t_TypeOfEquipment.Find(id);
            if (t_TypeOfEquipment == null)
            {
                //return NotFound();
                return db.t_TypeOfEquipment.Select(s => new TypeofEquipmentModel() { TypeId = s.TypeId, Name = s.Name, Effective = s.Effective, Warranty = s.Warranty, Limit = s.Limit }).AsQueryable(); 
            }

            db.t_TypeOfEquipment.Remove(t_TypeOfEquipment);
            db.SaveChanges();

            //return Ok(t_TypeOfEquipment);
            return db.t_TypeOfEquipment.Select(s => new TypeofEquipmentModel() { TypeId = s.TypeId, Name = s.Name, Effective = s.Effective, Warranty = s.Warranty, Limit = s.Limit }).AsQueryable(); 
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool t_TypeOfEquipmentExists(int id)
        {
            return db.t_TypeOfEquipment.Count(e => e.TypeId == id) > 0;
        }
    }
}