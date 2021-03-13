using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TExp.Models;

namespace TExp.Controllers
{
    public class WizardStepOneController : ApiController
    {
        private readonly TExpEntities db;

        public WizardStepOneController()
        {
            db = new TExpEntities();
        }

        [Route("api/WizardStepOne")]
        public IQueryable<TypeofEquipmentModel> Get()
        {
            /*t_TypeOfEquipment typeOfEquipment = new t_TypeOfEquipment();*/
            IQueryable<TypeofEquipmentModel> typeofEquipmentModel = db.t_TypeOfEquipment.Select(s => new TypeofEquipmentModel() { TypeId = s.TypeId, Name = s.Name, Warranty = s.Warranty, Effective = s.Effective, Limit = s.Limit }).AsQueryable();
            return typeofEquipmentModel;
        }

        // GET: api/Wizard/5
        [Route("api/WizardStepOne/{id}")]
        public EquipmentTypeOfEquipmentModel Get(int id)
        {
            t_Equipment equipment = new t_Equipment();
            equipment = db.t_Equipment.Find(id);
            EquipmentModel equipmentModel = db.t_Equipment.Where(w => w.EquipmentId == id).Select(s => new EquipmentModel() { Name = s.Name, StartupDate = s.StartupDate, Price = s.Price, InventoryNumber = s.InventoryNumber, SerialNumber = s.SerialNumber }).FirstOrDefault();
            selectTypeOfEquipmentModel selectedTypeOfEquipment = equipment.t_TypeOfEquipment.Select(s => new selectTypeOfEquipmentModel() { TypeId = s.TypeId, Name = s.Name }).FirstOrDefault(); //).AsQueryable(); //.Join(db.t_TypeOfEquipment, sw => sw.TypeId, s => s.TypeId, (sw, s) => s.TypeId).FirstOrDefault();
            //Debug.WriteLine("selectedTypeOfEquipment=" + selectedTypeOfEquipment.ToString());
            EquipmentTypeOfEquipmentModel equipmentTypeOfEquipmentModel = new EquipmentTypeOfEquipmentModel();
            equipmentTypeOfEquipmentModel.equipment = equipmentModel;
            equipmentTypeOfEquipmentModel.selectTypeOfEquipment = selectedTypeOfEquipment;
            return equipmentTypeOfEquipmentModel;
        }

        [Route("api/WizardStepOne/{id}/{typeId}")]
        public void Put(int id, int typeId)
        {
            t_Equipment equipment = db.t_Equipment.Find(id);
            var selectedEquipments = equipment.t_TypeOfEquipment.ToList();
            if (selectedEquipments != null)
            {
                foreach (t_TypeOfEquipment selectedEquipment in selectedEquipments)
                {
                    equipment.t_TypeOfEquipment.Remove(selectedEquipment);
                    db.SaveChanges();
                }
            }
            t_TypeOfEquipment typeOfEquipment = db.t_TypeOfEquipment.Find(typeId);
            if (typeOfEquipment != null)
            {
                equipment.t_TypeOfEquipment.Add(typeOfEquipment);
                db.SaveChanges();
            }
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
