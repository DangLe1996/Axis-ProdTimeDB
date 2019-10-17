﻿using Axis_ProdTimeDB.DAL;
using System;
using System.Data;
using System.Linq;
namespace Axis_ProdTimeDB.InputClasses
{
    class DustCover : Utilities
    {
        public DustCover(string paramFilePath)
        {
            string optionName = this.GetType().Name;
            var dt = ConvertCSVtoDataTable(paramFilePath);
            var newSort = (from row in dt.AsEnumerable()

                           group row by new
                           {
                               Length = row.Field<string>("Length"),

                               Workcenter = row.Field<string>("Work Center")
                           } into grp
                           //orderby grp.Key
                           select new
                           {

                               length = Int32.Parse(grp.Key.Length),
                               workcenter = grp.Key.Workcenter,

                               Sum = grp.Sum(r => Double.Parse(r.Field<string>("Time (min)")))
                           }).ToList();
            foreach (var row in newSort)
            {

                OptionTB.AddInstance(optionName, row.Sum, row.length);
                using (var db = new TimeContext())
                {
                    foreach (var fixture in db.Prod.Where(r => r.Type == fixturetype).ToList())
                    {
                        ProdTB.AddOption(fixturetype, fixture.Code, row.workcenter, optionName, row.Sum, row.length);

                    }

                }
            }
        }
    }
}
