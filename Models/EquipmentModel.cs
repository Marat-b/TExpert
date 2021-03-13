using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TExp.Models
{
    public class EquipmentModel
    {
        public int EquipmentId { get; set; }
        public string Name { get; set; }
        public DateTime? StartupDate { get; set; }
        public decimal? Price { get; set; }
        public string InventoryNumber { get; set; }
        public string SerialNumber { get; set; }
    }

    public class EquipmentTypeOfEquipmentModel
    {
        public EquipmentModel equipment;
        public selectTypeOfEquipmentModel selectTypeOfEquipment;
    }
}