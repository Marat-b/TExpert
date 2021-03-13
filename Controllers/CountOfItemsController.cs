using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using TExp.Models;
#if DEBUG
using System.Diagnostics;
#endif

namespace TExp.Controllers
{
    public class CountOfItemsController : ApiController
    {
        private TExpEntities db;

        public CountOfItemsController()
        {
            db = new TExpEntities();
        }

        [HttpGet]
        public string Get(int id)
        {

#if DEBUG
            Debug.WriteLine("*** Begin Get ****");
            
#endif
            EquipmentIdModel equipmentId = new EquipmentIdModel();
            
            List<EquipmentIdModel> equipmentsId = new List<EquipmentIdModel>();
            
            int CountOfEquipments = 0;
            if (HttpContext.Current.Session["equipments"] != null)
            {
#if DEBUG
                Debug.WriteLine("Session is NOT null!");
                
#endif
                equipmentsId = HttpContext.Current.Session["equipments"] as List<EquipmentIdModel>;
#if DEBUG
                foreach(EquipmentIdModel item in equipmentsId)
                {
                    Debug.WriteLine("item.EquipmentId=" + item.EquipmentId.ToString());
                }
#endif
                CountOfEquipments = equipmentsId.Count();
            }
            

            return CountOfEquipments.ToString();
        }
    }
}
