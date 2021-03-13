using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TExp.Models
{
    public class ExpertiseModel
    {
        public int ExpertiseId { get; set; }
        public string Staff { get; set; }
        public string Reason { get; set; }
        public string Conclusion { get; set; }
        public int? NumberExp { get; set; }
        public DateTimeOffset? DateExp { get; set; }
        public string Staff2 { get; set; }
        public string RequestId { get; set; }
        public bool? IsServiceableEquipment { get; set; }
        public bool? IsWarrantyRepair { get; set; }
        public bool? IsOrganizationRepair { get; set; }
        public bool? IsPartsSupply { get; set; }
        public bool? IsServiceable { get; set; }
        public bool? IsForWriteoff { get; set; }

    }
}