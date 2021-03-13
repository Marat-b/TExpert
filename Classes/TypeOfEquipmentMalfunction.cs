using System;
using System.Collections.Generic;
using System.Linq;
//using System.Web.Http;
using System.Web;
using TExp.Models;

namespace TExp.Classes
{
    public class TypeOfEquipmentMalfunctionOps
    {
        private readonly TExpEntities db;
        private t_TypeOfEquipment typeOfEquipment;

        public TypeOfEquipmentMalfunctionOps(int TypeID)
        {
            db = new TExpEntities();
            typeOfEquipment = db.t_TypeOfEquipment.Find(TypeID);
        }

        public IList<int> GetMalfunctions()
        {
            return typeOfEquipment.t_Malfunction.Join(db.t_Malfunction, sw => sw.FaultId, s => s.FaultId, (sw, s) => s.FaultId).ToList();
        }

        public void AddToTypeOfEquipmentMalfunction(int FaultId)
        {
            var malfunction = db.t_Malfunction.SingleOrDefault(s => s.FaultId == FaultId);

            if (malfunction!=null)
            {
                typeOfEquipment.t_Malfunction.Add(malfunction);
                db.SaveChanges();
            }
        }

        public void RemoveFromTypeOfEquipmentMalfunction(int FaultId)
        {
            var malfunction = typeOfEquipment.t_Malfunction.SingleOrDefault(s => s.FaultId == FaultId);

            if (malfunction!=null)
            {
                typeOfEquipment.t_Malfunction.Remove(malfunction);
                db.SaveChanges();
            }
        }
        /*
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }*/
    }
}