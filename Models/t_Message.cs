//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TExp.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class t_Message
    {
        public int MessageId { get; set; }
        public string Limit { get; set; }
        public string Effective { get; set; }
        public string Deadline { get; set; }
        public int CostRepair { get; set; }
        public string OverCostRepair { get; set; }
        public Nullable<int> LastNumber { get; set; }
    }
}