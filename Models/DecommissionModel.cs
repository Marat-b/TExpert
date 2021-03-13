using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TExp.Models
{
    public class DecommissionModel
    {
        public bool StateOfDecommission { get; set; }
        public DateTime? DateOfDecommission { get; set; }
    }
}