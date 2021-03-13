using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TExp.Models
{
    public class ActOfExpertiseModel
    {
        public EquipmentModel equipment { get; set; }
        public ExpertiseModel expertise { get; set; }
    }
}