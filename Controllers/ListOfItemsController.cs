using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
#if DEBUG
using System.Diagnostics;
#endif
using TExp.Models;

namespace TExp.Controllers
{
   
    public class ListOfItemsController : ApiController
    {
        private TExpEntities db;

        public ListOfItemsController()
        {
            db = new TExpEntities();
        }

        
        // GET: api/ListOfItems
        //[Route("api/sessions/ListOfItems/{id}")]
        [HttpGet]
        public IQueryable<EquipmentModel> Get(int id)
        {
#if DEBUG
            Debug.WriteLine("*** Begin Get ******");
#endif
            //List<int> equipmentsId = new List<int>();
            //int[] equipmentsId=new int[]{};
            //List<EquipmentIdModel> equipmentsId = new List<EquipmentIdModel>();
            IEnumerable<EquipmentIdModel> equipmentsId = HttpContext.Current.Session["equipments"] as  IEnumerable<EquipmentIdModel>;
            string[] equipmentsIdA;

            //if (equipmentsId.Select(s => s.EquipmentId).Count()!=0)
            if (equipmentsId!=null)
            {
                 equipmentsIdA = equipmentsId.Select(s => s.EquipmentId.ToString()).ToArray();   
            }
            else
            {
                equipmentsIdA = new string[] {""};
            }
            
            //var equipmentsId=new string[] {"56","240","241"};
            //List<EquipmentModel> equipmentsId = new List<EquipmentModel>();
            //equipmentsId = HttpContext.Current.Session["equipments"] as List<EquipmentIdModel>;
            //equipmentsId.to
#if DEBUG
            /*foreach (EquipmentIdModel i in equipmentsId)
            {
                Debug.WriteLine(String.Format("EquipmentID={0}",i.EquipmentId.ToString()));

            }*/
            foreach (string i in equipmentsIdA)
            {
                Debug.WriteLine(String.Format("EquipmentID={0}", i.ToString()));

            }
#endif
            /*IQueryable<EquipmentModel> equipmentModel = db.t_Equipment.Select(s => new EquipmentModel()
            {
                EquipmentId = s.EquipmentId,
                InventoryNumber = s.InventoryNumber,
                Name = s.Name,
                Price = s.Price,
                SerialNumber = s.SerialNumber,
                StartupDate = s.StartupDate
           
            }).Where(w =>w.EquipmentId.Equals(equipmentsId.Select(se=>se.EquipmentId))).AsQueryable();*/
            //}).Any(u => equipmentsId.Select(se => se.EquipmentId).Equals(u.EquipmentId));
            //equipmentsId.Select(ss => ss.EquipmentId).Contains(w.EquipmentId)
            //}).Join(equipmentsId,sw=>sw.EquipmentId,w=>w.EquipmentId,(sw,w)=>sw.EquipmentId) ).AsQueryable(); //equipmentsId.Contains(w.EquipmentId)

            /*IQueryable<EquipmentModel> equipmentModel = (from t in db.t_Equipment
                                                         join em in equipmentsId
                                                             on t.EquipmentId equals em.EquipmentId
                                                         select new EquipmentModel()
                                                         {
                                                             EquipmentId = t.EquipmentId,
                                                             InventoryNumber = t.InventoryNumber,
                                                             Name = t.Name,
                                                             Price = t.Price,
                                                             SerialNumber = t.SerialNumber,
                                                             StartupDate = t.StartupDate
                                                         }).AsQueryable();*/
            IQueryable<EquipmentModel> equipmentModel = (from t in db.t_Equipment 
                                                        // where equipmentsIdA.Equals(t.EquipmentId.ToString())
                                                         //Select(s=>s.EquipmentId)
                                                         where equipmentsIdA.Contains(t.EquipmentId.ToString())
                                                         select new EquipmentModel()
                                                         {
                                                             EquipmentId = t.EquipmentId,
                                                             InventoryNumber = t.InventoryNumber,
                                                             Name = t.Name,
                                                             Price = t.Price,
                                                             SerialNumber = t.SerialNumber,
                                                             StartupDate = t.StartupDate
                                                         }).AsQueryable();
#if DEBUG
            Debug.WriteLine("Count=" + equipmentModel.Count().ToString());
            /*foreach(EquipmentModel item in equipmentModel)
            {
                Debug.WriteLine(String.Format("EquipmentId={0}, Name={1}",item.EquipmentId,item.Name));
            }*/
#endif
            return equipmentModel;
        }

        // GET: api/ListOfItems/5
        /*public string Get(int id)
        {
            return "value";
        }

        // POST: api/ListOfItems
        public void Post([FromBody]string value)
        {
        }*/

        // PUT: api/ListOfItems/5
        //[Route("api/sessions/ListOfItems/{id}")]
        [HttpPut]
        public string Put(int id )
        {
#if DEBUG
            Debug.WriteLine("*** Begin Put ****");
            Debug.WriteLine("id="+id.ToString());
#endif
            EquipmentIdModel equipmentId = new EquipmentIdModel();
            equipmentId.EquipmentId=id;
            List<EquipmentIdModel> equipmentsId=new List<EquipmentIdModel>();
            //int[] equipmentsId = new int[] { };
            int CountOfEquipments = 0;
            if (HttpContext.Current.Session["equipments"]!=null)
            {
#if DEBUG
                Debug.WriteLine("Session is NOT null!");
                
#endif
                equipmentsId =  HttpContext.Current.Session["equipments"] as List<EquipmentIdModel>;
#if DEBUG
                foreach(EquipmentIdModel item in equipmentsId)
                {
                    Debug.WriteLine("item.EquipmentId=" + item.EquipmentId.ToString());
                }
#endif
                if (equipmentsId.Where(w=>w.EquipmentId==id).Count()==0)
                {
#if DEBUG
                    Debug.WriteLine("!equipmentsId.Contains(equipmentId)");
#endif
                    equipmentsId.Add(equipmentId);
                    CountOfEquipments = equipmentsId.Count();
                    HttpContext.Current.Session["equipments"] = equipmentsId;
                }
                else
                {
                    CountOfEquipments = equipmentsId.Count();
                }
            }
            else
            {
#if DEBUG
                Debug.WriteLine("Session is  null!");
#endif
                equipmentsId.Add(equipmentId);
                CountOfEquipments = equipmentsId.Count();
                HttpContext.Current.Session["equipments"] = equipmentsId;
            }

            return CountOfEquipments.ToString();
        }

        // DELETE: api/ListOfItems/5
        [HttpDelete]
        public IQueryable<EquipmentModel> Delete(int id)
        {
#if DEBUG
            Debug.WriteLine("****** Delete **********");
            Debug.WriteLine("id=" + id.ToString());
#endif
            List<EquipmentIdModel> equipmentsId = new List<EquipmentIdModel>();
            equipmentsId = HttpContext.Current.Session["equipments"] as List<EquipmentIdModel>;
            //EquipmentIdModel equipment=new EquipmentIdModel();
            //equipment.EquipmentId=id;
            //equipmentsId.Remove(equipment);
            equipmentsId.Remove(equipmentsId.First(f => f.EquipmentId == id));
            string[] equipmentsIdA;
            if (equipmentsId != null)
            {
#if DEBUG
                Debug.WriteLine("equipmentsId is NOT null");
                Debug.WriteLine("equipmentsId.Count="+equipmentsId.Count().ToString());
#endif
                equipmentsIdA = equipmentsId.Select(s => s.EquipmentId.ToString()).ToArray();
                HttpContext.Current.Session["equipments"] = equipmentsId;
            }
            else
            {
#if DEBUG
                Debug.WriteLine("equipmentsId is null");
#endif

                equipmentsIdA = new string[] { "" };
                HttpContext.Current.Session["equipments"] = null;
            }

            IQueryable<EquipmentModel> equipmentModel = (from t in db.t_Equipment
                                                         
                                                         where equipmentsIdA.Contains(t.EquipmentId.ToString())
                                                         select new EquipmentModel()
                                                         {
                                                             EquipmentId = t.EquipmentId,
                                                             InventoryNumber = t.InventoryNumber,
                                                             Name = t.Name,
                                                             Price = t.Price,
                                                             SerialNumber = t.SerialNumber,
                                                             StartupDate = t.StartupDate
                                                         }).AsQueryable();
            return equipmentModel;
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
