using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http.Description;
using System.Web.Http;
using TExp.Models;
using System.Data;
using System.Data.Entity;
#if DEBUG
using System.Diagnostics;
#endif

namespace TExp.Controllers
{
    public class EquipmentController : ApiController
    {

        private TExpEntities db;
        public EquipmentController()
        {
            db = new TExpEntities();
        }

        // GET: api/Equipment
        /*public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }*/

        // GET: api/Equipment/5
        [Route("api/Equipment/{id}")]
        [ResponseType(typeof(EquipmentModelInput))]
        public IHttpActionResult Get(int id)
        {
#if DEBUG
            Debug.WriteLine("id="+id.ToString());
#endif
            EquipmentModelInput equipment = db.t_Equipment.Where(w => w.EquipmentId == id).Select(s => new EquipmentModelInput() {
            EquipmentId=s.EquipmentId,
            InventoryNumber=s.InventoryNumber,
            Name=s.Name,
            Price=s.Price,
            SerialNumber=s.SerialNumber,
            StartupDate=s.StartupDate.ToString(),
            MOL=s.MOL,
            MaterialAccount=s.MaterialAccount
            }).FirstOrDefault();
            if (equipment==null)
            {
                return NotFound();
            }
            return Ok(equipment);
            //return "value";
        }

        // POST: api/Equipment
        public IHttpActionResult Post([FromBody]EquipmentModelInput equipmentModelInput)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
#if DEBUG
            Debug.WriteLine("Model is valid!");
            Debug.WriteLine("equipmentModelInput.Name=" + equipmentModelInput.Name);
            Debug.WriteLine("equipmentModelInput.Price=" + equipmentModelInput.Price.ToString());
#endif
            t_Equipment equipment = new t_Equipment();
            equipment.EquipmentId = 0;
            equipment.InventoryNumber = equipmentModelInput.InventoryNumber;
            equipment.MaterialAccount = equipmentModelInput.MaterialAccount;
            equipment.MOL = equipmentModelInput.MOL;
            equipment.Name = equipmentModelInput.Name;
            equipment.Price = equipmentModelInput.Price;
            equipment.SerialNumber = equipmentModelInput.SerialNumber;
            equipment.StartupDate = Convert.ToDateTime(equipmentModelInput.StartupDate);
            //equipment.StartupDate = (System.DateTime?) DateTime.Parse(equipmentModelInput.StartupDate).ToShortDateString();
            db.t_Equipment.Add(equipment);
            db.SaveChanges();
            return Ok();
        }

        // PUT: api/Equipment/5
        [Route("api/Equipment/{id}")]
        public IHttpActionResult Put(int id, [FromBody]EquipmentModelInput equipmentModelInput)
        {
#if DEBUG
            Debug.WriteLine("I'm in PUT");
#endif
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
#if DEBUG
            Debug.WriteLine("Put id="+id.ToString());
#endif
            t_Equipment equipment = new t_Equipment();
            equipment.EquipmentId = equipmentModelInput.EquipmentId;
            equipment.InventoryNumber = equipmentModelInput.InventoryNumber;
            equipment.MaterialAccount = equipmentModelInput.MaterialAccount;
            equipment.MOL = equipmentModelInput.MOL;
            equipment.Name = equipmentModelInput.Name;
            equipment.Price = equipmentModelInput.Price;
            equipment.SerialNumber = equipmentModelInput.SerialNumber;
            equipment.StartupDate = Convert.ToDateTime(equipmentModelInput.StartupDate);
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

        // DELETE: api/Equipment/5
        /*public void Delete(int id)
        {
        }*/

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
