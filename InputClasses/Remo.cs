using Axis_ProdTimeDB.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Axis_ProdTimeDB.InputClasses
{
    class Remo:Program
    {

        public Remo(string paramFilePath)
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
               
                
                using (var db = new TimeContext())
                {
                    foreach (var fixture in db.Fixtures.Where(r => r.WorkCenter == row.workcenter))
                    {
                        FixtureTB.AddOption(fixture.FxCode, row.workcenter, optionName, row.Sum);

                    }

                }

            }

        }
    }
}