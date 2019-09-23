using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axis_ProdTimeDB.InputClasses
{
    class WiresQty:Program
    {
        public WiresQty()
        {
            OptionTB.AddInstance("WiresQty", 4);
            OptionTB.AddParam("wiresQty", 4, "Driver", "BI");
            OptionTB.AddInstance("WiresQty", 3);
            OptionTB.AddParam("wiresQty", 3, "Driver", "E");
            OptionTB.AddParam("wiresQty", 3, "Driver", "ERS");

            OptionTB.AddInstance("WiresQty", 5);
            OptionTB.AddParam("wiresQty", 5, "Driver", "D");
            OptionTB.AddParam("wiresQty", 5, "Driver", "DP");
            OptionTB.AddParam("wiresQty", 5, "Driver", "MD");

            OptionTB.AddInstance("WiresQty", 6);
            OptionTB.AddParam("wiresQty", 6, "Driver", "LT");

            
        }
    }
}
