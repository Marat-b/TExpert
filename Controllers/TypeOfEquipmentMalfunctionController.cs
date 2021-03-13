using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using TExp.Models;
using System.Diagnostics;
using TExp.Classes;

namespace WebAPITest2.Controllers
{
    public class TypeOfEquipmentMalfunctionController : ApiController
    {
        private TExpEntities db; // = new TExpEntities();

        public TypeOfEquipmentMalfunctionController()
        {
            db = new TExpEntities();
            //db.Configuration.ProxyCreationEnabled = false;
        }
        // GET: api/TypeOfEquipmentMalfunction
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/TypeOfEquipmentMalfunction/5
        public TypeOfEquipmentMalfunctionModel Get(int id)
        {
            TypeOfEquipmentMalfunctionModel typeOfEquipmentMalfunction = new TypeOfEquipmentMalfunctionModel();
            //t_Malfunction malfunction; // = new malfunction();
            t_TypeOfEquipment typeOfEquipment=db.t_TypeOfEquipment.Where(w=>w.TypeId==id).FirstOrDefault();
#if DEBUG
            Debug.WriteLine("typeOfEquipment.Name=" + typeOfEquipment.Name + " typeOfEquipment.TypeId="+ typeOfEquipment.TypeId.ToString());
#endif
            //typeOfEquipmentMalfunction.Malfunctions= (malfunction.t_TypeOfEquipment.Where(s => s.TypeId == id)).ToList();
            //typeOfEquipmentMalfunction.selectedMalfunctions = (typeOfEquipment.ref_Malfunction).ToList();
            IList<int> typeOfEquipments=( typeOfEquipment.t_Malfunction.Join(db.t_Malfunction, sw => sw.FaultId, s => s.FaultId, (sw, s) => s.FaultId).ToList());
#if DEBUG

            Debug.WriteLine("typeOfEquipments.count=" + typeOfEquipments.Count.ToString());
#endif
            //typeOfEquipmentMalfunction.selectedMalfunctions = db.t_Malfunction.Where(w => typeOfEquipments.Contains(w.FaultId)).Select(s=>new t_Malfunction{FaultId=s.FaultId,Name=s.Name}).ToList();
            typeOfEquipmentMalfunction.selectedMalfunctions = (from m in db.t_Malfunction where typeOfEquipments.Contains(m.FaultId) select (new MalfunctionModel() {FaultId=m.FaultId,Name=m.Name })).ToList(); //typeOfEquipments.Contains(1)
            typeOfEquipmentMalfunction.Malfunctions = db.t_Malfunction.Select(s=>new MalfunctionModel(){FaultId=s.FaultId,Name=s.Name}).ToList();
            typeOfEquipmentMalfunction.TypeId = id;
            //typeOfEquipmentMalfunction.Malfunctions = db.t_TypeOfEquipment;
#if DEBUG
            Debug.WriteLine("Начало выбора Malfunction");
            foreach(MalfunctionModel item in typeOfEquipmentMalfunction.selectedMalfunctions)
            {
                Debug.WriteLine("FaultId={0},Name={1}", item.FaultId.ToString(), item.Name);
            }
#endif
            return typeOfEquipmentMalfunction;
        }

        // POST: api/TypeOfEquipmentMalfunction
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/TypeOfEquipmentMalfunction/5
        public IHttpActionResult Put(int id, IEnumerable<t_Malfunction> malfunction)
        {
#if DEBUG
            Debug.WriteLine("id=" + id.ToString());
            foreach(t_Malfunction item in malfunction)
            {
                Debug.WriteLine("FaultId={0},Name={1}",item.FaultId.ToString(),item.Name);
            }
#endif
            IEnumerable<int> malfunctions = malfunction.Select(s => s.FaultId);
            TypeOfEquipmentMalfunctionOps typeOfEquipmentMalfunctionOps = new TypeOfEquipmentMalfunctionOps(id);
            var EinF = typeOfEquipmentMalfunctionOps.GetMalfunctions();
            //IEnumerable<t_Malfunction> faults = (from f in malfunction where !EinF.Contains(f) select f).ToList();
            IList<int> faults = malfunctions.Except(EinF).ToArray<int>();
#if DEBUG
            foreach(int item in faults)
            {
                Debug.WriteLine("faults FaultId="+ item.ToString());
            }
#endif
            foreach(var fault in faults.Where(fault=>!EinF.Contains(fault)))
            {
#if DEBUG
                Debug.WriteLine("fault FaultID="+ fault.ToString());
#endif
                typeOfEquipmentMalfunctionOps.AddToTypeOfEquipmentMalfunction(fault);
            }
            
            IEnumerable<int> faults2 = EinF.Except(malfunctions).ToArray<int>();
            //IEnumerable<t_Malfunction> faults2 = (from f in malfunction where EinF.Contains(f) select f).ToList();
#if DEBUG
            foreach (int item in faults2)
            {
                Debug.WriteLine("faults2 FaultId="+ item.ToString());
            }
#endif

            foreach(var fault2 in faults2.Where(w=>EinF.Contains(w)))
            {
#if DEBUG
                Debug.WriteLine("fault2 FaultID=" + fault2.ToString());
#endif
                typeOfEquipmentMalfunctionOps.RemoveFromTypeOfEquipmentMalfunction(fault2);

            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // DELETE: api/TypeOfEquipmentMalfunction/5
        public void Delete(int id)
        {
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
