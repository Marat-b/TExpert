using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Diagnostics;
using TExp.Models;
using TExp.Classes;

namespace TExp.Controllers
{
    public class WizardStepTwoController : ApiController
    {
        private readonly TExpEntities db;

        public WizardStepTwoController()
        {
            db = new TExpEntities();
        }


        /// <summary>
        /// Get for StepTwo
        /// </summary>
        /// <param name="id">TypeId</param>
        /// <param name="equipmentId"></param>
        /// <returns></returns>
        [Route("api/WizardStepTwo/{id}/{equipmentId}")]
        public EquipmentMalfunctionModel Get(int id, int equipmentId)
        {
            EquipmentMalfunctionModel equipmentMalfunction = new EquipmentMalfunctionModel();
            //t_Malfunction malfunction; // = new malfunction();
            t_TypeOfEquipment typeOfEquipment = db.t_TypeOfEquipment.Where(w => w.TypeId == id).FirstOrDefault();
            t_Equipment equipment = db.t_Equipment.Find(equipmentId);
#if DEBUG
            Debug.WriteLine("typeOfEquipment.Name=" + typeOfEquipment.Name + " typeOfEquipment.TypeId=" + typeOfEquipment.TypeId.ToString());
#endif
            //typeOfEquipmentMalfunction.Malfunctions= (malfunction.t_TypeOfEquipment.Where(s => s.TypeId == id)).ToList();
            //typeOfEquipmentMalfunction.selectedMalfunctions = (typeOfEquipment.ref_Malfunction).ToList();

            IList<int> typeOfEquipments = (equipment.t_Malfunction.Join(db.t_Malfunction, sw => sw.FaultId, s => s.FaultId, (sw, s) => s.FaultId).ToList());
#if DEBUG

            Debug.WriteLine("typeOfEquipments.count=" + typeOfEquipments.Count.ToString());
#endif
            equipmentMalfunction.selectedMalfunctions = db.t_Malfunction.Where(w => typeOfEquipments.Contains(w.FaultId)).Select(s => new MalfunctionModel() { FaultId = s.FaultId, Name = s.Name }).ToList();
            //typeOfEquipmentMalfunction.selectedMalfunctions = (from m in db.t_Malfunction where typeOfEquipments.Contains(m.FaultId) select new t_Malfunction() {FaultId=m.FaultId,Name=m.Name }).ToList(); //typeOfEquipments.Contains(1)
            equipmentMalfunction.Malfunctions = typeOfEquipment.t_Malfunction.Select(s => new MalfunctionModel() { FaultId = s.FaultId, Name = s.Name }).ToList();     //db.t_Malfunction.ToList(); .Select(s => new t_Malfunction() { FaultId = s.FaultId, Name = s.Name })
            //equipmentMalfunction.TypeId = id;
            //typeOfEquipmentMalfunction.Malfunctions = db.t_TypeOfEquipment;
#if DEBUG
            Debug.WriteLine("Начало выбора Malfunction");
            foreach (MalfunctionModel item in equipmentMalfunction.selectedMalfunctions)
            {
                Debug.WriteLine("FaultId={0},Name={1}", item.FaultId.ToString(), item.Name);
            }
#endif
            return equipmentMalfunction;
        }

        /// <summary>
        /// Put for StepTwo
        /// </summary>
        /// <param name="id">Id of equipment</param>
        /// <param name="malfunction">Selected malfunctions for this equipment</param>
        /// <returns></returns>
        [Route("api/WizardStepTwo/{id}")]
        public IHttpActionResult Put(int id, IEnumerable<t_Malfunction> malfunction)
        {
#if DEBUG
            Debug.WriteLine("id=" + id.ToString());
            foreach (t_Malfunction item in malfunction)
            {
                Debug.WriteLine("FaultId={0},Name={1}", item.FaultId.ToString(), item.Name);
            }
#endif
            IEnumerable<int> malfunctions = malfunction.Select(s => s.FaultId);
            EquipmentMalfunctionOps equipmentMalfunctionOps = new EquipmentMalfunctionOps(id);
            var EinF = equipmentMalfunctionOps.GetMalfunctions();
            //IEnumerable<t_Malfunction> faults = (from f in malfunction where !EinF.Contains(f) select f).ToList();
            IList<int> faults = malfunctions.Except(EinF).ToArray<int>();
#if DEBUG
            foreach (int item in faults)
            {
                Debug.WriteLine("faults FaultId=" + item.ToString());
            }
#endif
            foreach (var fault in faults.Where(fault => !EinF.Contains(fault)))
            {
#if DEBUG
                Debug.WriteLine("fault FaultID=" + fault.ToString());
#endif
                equipmentMalfunctionOps.AddToEquipmentMalfunction(fault);
            }

            IEnumerable<int> faults2 = EinF.Except(malfunctions).ToArray<int>();
            //IEnumerable<t_Malfunction> faults2 = (from f in malfunction where EinF.Contains(f) select f).ToList();
#if DEBUG
            foreach (int item in faults2)
            {
                Debug.WriteLine("faults2 FaultId=" + item.ToString());
            }
#endif

            foreach (var fault2 in faults2.Where(w => EinF.Contains(w)))
            {
#if DEBUG
                Debug.WriteLine("fault2 FaultID=" + fault2.ToString());
#endif
                equipmentMalfunctionOps.RemoveFromEquipmentMalfunction(fault2);

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
