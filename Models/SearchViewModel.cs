using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TExp.Models
{
    public class SearchViewModel
    {
        [Key]
        public int EquipmentId { get; set; }
        public string Name { get; set; }
        public string SerialNumber { get; set; }
        public string InventoryNumber { get; set; }
        public System.Decimal Price { get; set; }
        public System.DateTime? StartupDate { get; set; }
        public string MOL { get; set; }
        public int hasExpertise { get; set; }
        public bool Decommission { get; set; }
    }
}