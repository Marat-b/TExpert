using System;
using System.Collections.Generic;


namespace TExp.Models
{
    public class EquipmentModelInput
    {
        public int EquipmentId { get; set; }
        public string MaterialAccount { get; set; }
        public string Name { get; set; }
        public string InventoryNumber { get; set; }
        public string SerialNumber { get; set; }
        public decimal Price { get; set; }
        public string StartupDate { get; set; }
        public string MOL { get; set; }
    }
}