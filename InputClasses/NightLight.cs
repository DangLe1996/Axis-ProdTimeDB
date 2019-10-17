using Axis_ProdTimeDB.DAL;
using System;
using System.Data;
using System.Linq;

namespace Axis_ProdTimeDB.InputClasses
{
    class Nightlight : Utilities
    {
        public Nightlight(string paramFilePath)
        {
            string optionName = this.GetType().Name;
            var dt = ConvertCSVtoDataTable(paramFilePath);

            var newSort = (from row in dt.AsEnumerable()

                           group row by new
                           {
                               ID = row.Field<string>("Fixture Type"),


                               Workcenter = row.Field<string>("Work Center")
                           } into grp
                           //orderby grp.Key
                           select new
                           {
                               Fixture = grp.Key.ID,

                               workcenter = grp.Key.Workcenter,
                               Sum = grp.Sum(r => Double.Parse(r.Field<string>("Time(min)")))
                           }).ToList();


            using (var db = new TimeContext())
            {
                foreach (var row in newSort)
                {

                    ProdTB.AddInstance(fixturetype, row.Fixture, row.workcenter);
                    OptionTB.AddInstance(optionName, row.Sum);

                    ProdTB.AddOption(fixturetype, row.Fixture, row.workcenter, optionName, row.Sum);

                }


            }


        }
    }
}
