using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TExp.Models
{
    public class EquipmentMalfunctionModel
    {
        public IEnumerable<MalfunctionModel> selectedMalfunctions { get; set; }
        public IEnumerable<MalfunctionModel> Malfunctions { get; set; }
    }

}