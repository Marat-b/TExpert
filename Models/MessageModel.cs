using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TExp.Models
{
    public class MessageModel
    {
        public int MessageId { get; set; }
        public string Effective { get; set; }
        public string Limit {get; set;}
        public string Deadline {get;set;}
        public string OverCostRepair { get; set; }
        public int? CostRepair { get; set; }
        public int? LastNumber { get; set; }
    }
}