using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwitchesDll
{
    public class EquipmentRootObject
    {
        //Parent
        public SwitchDB? UpperSwitchDB { get; set; }
        public object Equipment { get; set; }
    }
}
