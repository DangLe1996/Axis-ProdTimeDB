﻿using System;
using System.Data;
using System.Linq;
namespace Axis_ProdTimeDB.InputClasses
{
    class CounterW : Utilities
    {
        public CounterW(string paramFilePath)
        {
            string optionName = this.GetType().Name;
            var dt = ConvertCSVtoDataTable(paramFilePath);
            var newSort = (from row in dt.AsEnumerable()

                           group row by new
                           {
                               ID = row.Field<string>("Product ID"),

                               Workcenter = row.Field<string>("Work Center")
                           } into grp
                           //orderby grp.Key
                           select new
                           {
                               Product = grp.Key.ID,

                               workcenter = grp.Key.Workcenter,

                               Sum = grp.Sum(r => Double.Parse(r.Field<string>("Time (min)")))
                           }).ToList();
            foreach (var row in newSort)
            {
                ProdTB.AddInstance(prodtype, row.Product, row.workcenter);
                OptionTB.AddInstance(optionName, row.Sum);
                ProdTB.AddOption(prodtype, row.Product, row.workcenter, optionName, row.Sum);


            }
        }
    }
}
