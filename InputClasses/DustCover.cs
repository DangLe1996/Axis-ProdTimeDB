using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Axis_ProdTimeDB.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
namespace Axis_ProdTimeDB.InputClasses
{
    class DustCover:Program
    {
        public DustCover(string paramFilePath)
        {
            string optionName = "DustCover";
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
               
                OptionTB.AddInstance(optionName, row.Sum,row.length);
                using (var db = new TimeContext())
                {
                    foreach (var fixture in db.Fixtures)
                    {
                        FixtureTB.AddOption(fixture.FxCode, row.workcenter, optionName, row.Sum, row.length);

                    }

                }
            }
        }
    }
}
