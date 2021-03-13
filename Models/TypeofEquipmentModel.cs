using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TExp.Models
{
    public class TypeofEquipmentModel
    {
        public int TypeId { get; set; }
        public string Name { get; set; }
        public int Effective { get; set; }
        public int Warranty { get; set; }
        public int Limit { get; set; }
    }
}