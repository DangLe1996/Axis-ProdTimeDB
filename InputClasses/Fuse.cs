using Axis_ProdTimeDB.DAL;
using System;
using System.Data;
using System.Linq;

namespace Axis_ProdTimeDB.InputClasses
{
    class Fuse : Utilities
    {
        public Fuse(string paramFilePath)
        {

            string optionName = this.GetType().Name;
            var dt = ConvertCSVtoDataTable(paramFilePath);
            var newSort = (from row in dt.AsEnumerable()

                           group row by new
                           {


                               Workcenter = row.Field<string>("Work Center")
                           } into grp
                           select new
                           {
                               workcenter = grp.Key.Workcenter,
                               Sum = grp.Sum(r => Double.Parse(r.Field<string>("Time(min)")))
                           }).ToList();


            foreach (var row in newSort)
            {
                OptionTB.AddInstance(optionName, row.Sum);
                using (var db = new TimeContext())
                {

                    foreach (var fixture in db.Prod.Where(r => r.Type == fixturetype).ToList())
                    {
                        ProdTB.AddOption(fixturetype, fixture.Code, row.workcenter, optionName, row.Sum);
                    }
                }
            }






        }
    }

}
