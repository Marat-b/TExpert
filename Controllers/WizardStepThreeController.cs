using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.Entity;
using System.Diagnostics;
using TExp.Models;

namespace TExp.Controllers
{
    public class WizardStepThreeController : ApiController
    {
        private readonly TExpEntities db;

        public WizardStepThreeController()
        {
            db = new TExpEntities();
        }

        [Route("api/WizardStepThree/{id}/{price}")]
        public IHttpActionResult Put(int id, int price)
        {
            t_CostOfRepair costOfRepair = db.t_CostOfRepair.Find(id);
            //CostOfRepairModel costOfRepair = db.t_CostOfRepair.Where(w => w.EquipmentId == id).Select(s => new CostOfRepairModel() { EquipmentId = s.EquipmentId, Price = s.Price }).FirstOrDefault();
            if (costOfRepair == null)
            {
#if DEBUG
                Debug.WriteLine("costOfRepair == null");
#endif
                costOfRepair = new t_CostOfRepair();
                costOfRepair.EquipmentId = id;
                costOfRepair.Price = price;
                db.t_CostOfRepair.Add(costOfRepair);
                db.SaveChanges();
            }
            else
            {
#if DEBUG
                Debug.WriteLine(String.Format("costOfRepair id={0},price={1}", id, price));
#endif
                costOfRepair.Price = price;
                db.Entry(costOfRepair).State = EntityState.Modified;
                db.SaveChanges();
            }
            return StatusCode(HttpStatusCode.NoContent);
        }

        [Route("api/WizardStepThree/{id}")]
        public CostOfRepairModel Get(int id)
        {
            CostOfRepairModel costOfRepair = db.t_CostOfRepair.Where(w => w.EquipmentId == id).Select(s => new CostOfRepairModel() { EquipmentId = s.EquipmentId, Price = s.Price }).FirstOrDefault();
            return costOfRepair;
        }


    }
}
