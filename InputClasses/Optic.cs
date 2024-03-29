﻿using Axis_ProdTimeDB.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axis_ProdTimeDB.InputClasses
{
    class Optic : Program
    {
        public Optic(string paramFilePath)
        {
            string optionName = this.GetType().Name;
            var dt = ConvertCSVtoDataTable(paramFilePath);
            var newSort = (from row in dt.AsEnumerable()

                           group row by new
                           {
                               ID = row.Field<string>("Family Product"),
                               Optic = row.Field<string>("Optic"),
                               Length = row.Field<string>("Length"),
                               Workcenter = row.Field<string>("Work Center")
                           } into grp
                           select new
                           {
                               ProdFam = grp.Key.ID,
                               Optic = grp.Key.Optic,
                               Length = grp.Key.Length,
                               workcenter = grp.Key.Workcenter,
                               Sum = grp.Sum(r => Double.Parse(r.Field<string>("Time (min)")))
                           }).ToList();

            using (var db = new TimeContext())
            {
                foreach (var row in newSort)
                {

                   ProdFamTB.AddInstance(row.ProdFam, row.workcenter);


                    int length = Int32.Parse(row.Length);
                    OptionTB.AddInstance(optionName, row.Sum, length);

                    ParametersTB.AddInstance("Optic", row.Optic);


                    OptionTB.AddParam(optionName, row.Sum, "Optic", row.Optic);
                    ProdFamTB.AddOption(row.ProdFam, row.workcenter, optionName, row.Sum, length);

                    db.SaveChanges();
                }





            }
        }

    }
}
