using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TExp.Models;

namespace TExp.Classes
{
    public class EquipmentMalfunctionOps
    {
        private readonly TExpEntities db;
        private t_Equipment Equipment;

        public EquipmentMalfunctionOps(int EquipmentID)
        {
            db = new TExpEntities();
            Equipment = db.t_Equipment.Find(EquipmentID);
        }

        public IList<int> GetMalfunctions()
        {
            return Equipment.t_Malfunction.Join(db.t_Malfunction, sw => sw.FaultId, s => s.FaultId, (sw, s) => s.FaultId).ToList();
        }

        public void AddToEquipmentMalfunction(int FaultId)
        {
            var malfunction = db.t_Malfunction.SingleOrDefault(s => s.FaultId == FaultId);

            if (malfunction!=null)
            {
                Equipment.t_Malfunction.Add(malfunction);
                db.SaveChanges();
            }
        }

        public void RemoveFromEquipmentMalfunction(int FaultId)
        {
            var malfunction = Equipment.t_Malfunction.SingleOrDefault(s => s.FaultId == FaultId);

            if (malfunction!=null)
            {
                Equipment.t_Malfunction.Remove(malfunction);
                db.SaveChanges();
            }
        }
    }
}