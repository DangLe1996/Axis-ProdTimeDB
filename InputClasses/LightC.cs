﻿using Axis_ProdTimeDB.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axis_ProdTimeDB.InputClasses
{
    class LightC : Program
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

                ProdFamTB.AddInstance(row.ProdFam, row.workcenter);
                int length = Int32.Parse(row.Length);
                OptionTB.AddInstance(optionName, row.Sum, length);

                


                OptionTB.AddParam(optionName, row.Sum, "LampQty", row.LampQty);
                OptionTB.AddParam(optionName, row.Sum, "Circuit", row.circuit);
                ProdFamTB.AddOption(row.ProdFam, row.workcenter, optionName, row.Sum, length);

            }
                





                //}
            }

    }
}
