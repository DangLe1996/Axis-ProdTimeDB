using System;
using System.Data;
using System.Linq;

namespace Axis_ProdTimeDB.InputClasses
{
    class LightC : Utilities
    {
        public LightC(string paramFilePath)
        {
            string optionName = this.GetType().Name;
            var dt = ConvertCSVtoDataTable(paramFilePath);
            var newSort = (from row in dt.AsEnumerable()

                           group row by new
                           {
                               ID = row.Field<string>("Family Product"),
                               citcuit = row.Field<string>("Nb Circuit"),
                               Length = row.Field<string>("Length"),
                               Workcenter = row.Field<string>("Work Center"),
                               LampQty = row.Field<string>("Lamp/Cartridge Qty")
                           } into grp
                           select new
                           {
                               ProdFam = grp.Key.ID,
                               circuit = grp.Key.citcuit,
                               Length = grp.Key.Length,
                               LampQty = grp.Key.LampQty,
                               workcenter = grp.Key.Workcenter,
                               Sum = grp.Sum(r => Double.Parse(r.Field<string>("Time (min)")))
                           }).ToList();


            foreach (var row in newSort)
            {

                ProdTB.AddInstance(prodfamtype, row.ProdFam, row.workcenter);
                int length = Int32.Parse(row.Length);
                OptionTB.AddInstance(optionName, row.Sum, length);




                OptionTB.AddParam(optionName, row.Sum, "LampQty", row.LampQty);
                OptionTB.AddParam(optionName, row.Sum, "Circuits", row.circuit);

                ProdTB.AddOption(prodfamtype, row.ProdFam, row.workcenter, optionName, row.Sum, length);
            }






            //}
        }

    }
}
