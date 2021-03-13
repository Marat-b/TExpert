using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TExp.Models
{
    public class DocModel:ExpertiseModel
    {
        public int EquipmentId { get; set; }
        public string Name { get; set; }
        public string InventoryNumber { get; set; }
        public string SerialNumber { get; set; }
    }

    public class DocumentModel : DocModel
    {
        [Key]
        public int ExpertiseId { get; set; }
        public string RequestId { get; set; }
    }
}