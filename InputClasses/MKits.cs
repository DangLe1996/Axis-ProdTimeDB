using Axis_ProdTimeDB.DAL;
using System;
using System.Data;
using System.Linq;

namespace Axis_ProdTimeDB.InputClasses
{
    class MKits : Utilities
    {
        public MKits(string paramFilePath)
        {
            string optionName = this.GetType().Name;
            var dt = ConvertCSVtoDataTable(paramFilePath);

            var newSort = (from row in dt.AsEnumerable()

                           group row by new
                           {
                               ID = row.Field<string>("Fixture Type"),
                               Mouting = row.Field<string>("Mounting Suspension"),

                               Workcenter = row.Field<string>("Work Center")
                           } into grp
                           //orderby grp.Key
                           select new
                           {
                               Fixture = grp.Key.ID,
                               mounting = grp.Key.Mouting,

                               workcenter = grp.Key.Workcenter,
                               Sum = grp.Sum(r => Double.Parse(r.Field<string>("Total Time (min)")))
                           }).ToList();


            using (var db = new TimeContext())
            {
                foreach (var row in newSort)
                {

                    ProdTB.AddInstance(fixturetype, row.Fixture, row.workcenter);
                    OptionTB.AddInstance(optionName, row.Sum);
                    ParametersTB.AddInstance("Mounting", row.mounting);

                    OptionTB.AddParam(optionName, row.Sum, "Mounting", row.mounting);
                    ProdTB.AddOption(fixturetype, row.Fixture, row.workcenter, optionName, row.Sum);

                }


            }


        }
    }
}
