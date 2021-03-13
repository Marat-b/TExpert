using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TExp.Models;
using System.Diagnostics;

namespace TExp.Controllers
{
    public class ActOfExpertiseController : Controller
    {
        // GET: ActOfExpertise
        private readonly TExpEntities db;

        public ActOfExpertiseController()
        {
            db = new TExpEntities();
        }


        public ActionResult ActOfExpertise(int id)
        {
            //int id = 500;
            ActOfExpertiseModel actOfExpertise = new ActOfExpertiseModel();
            EquipmentModel equipment = db.t_Equipment.Where(w=>w.EquipmentId==id).Select(s => new EquipmentModel() {EquipmentId=s.EquipmentId,Name=s.Name,Price=s.Price,StartupDate=s.StartupDate,InventoryNumber=s.InventoryNumber,SerialNumber=s.SerialNumber }).SingleOrDefault();
            if (equipment==null)
            {
                return HttpNotFound();
            }
            ExpertiseModel expertise = db.t_Expertise.Where(w => w.EquipmentId == id).Select(s => new ExpertiseModel()
            {
                ExpertiseId = s.ExpertiseId, Conclusion = s.Conclusion,
                Reason = s.Reason, Staff = s.Staff,NumberExp = s.NumberExp,DateExp = s.DateExp,
                RequestId = s.RequestId,
                Staff2 = s.Staff2,
                IsServiceableEquipment = s.IsServiceableEquipment,
                IsWarrantyRepair = s.IsWarrantyRepair,
                IsOrganizationRepair = s.IsOrganizationRepair,
                IsPartsSupply = s.IsPartsSupply,
                IsServiceable = s.IsServiceable,
                IsForWriteoff = s.IsForWriteoff
                
            }).SingleOrDefault();

           

            if (expertise==null)
            {
                expertise = new ExpertiseModel();
                expertise.ExpertiseId=0;
                expertise.Conclusion="";
                expertise.Reason="";
                expertise.Staff="";
                expertise.NumberExp = 0;
                expertise.DateExp=DateTimeOffset.Now;
            }
#if DEBUG
            else
            {
                Debug.WriteLine("expertise.IsServiceableEquipment=" + expertise.IsServiceableEquipment.ToString());
            }
#endif
            actOfExpertise.equipment=equipment;
            actOfExpertise.expertise=expertise;
            return View(actOfExpertise);
        }
    }
}