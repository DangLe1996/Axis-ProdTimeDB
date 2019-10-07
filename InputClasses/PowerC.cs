using Axis_ProdTimeDB.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Axis_ProdTimeDB.InputClasses
{
    class PowerC:Utilities
    {

        public PowerC(string paramFilePath)
        {
            string optionName = this.GetType().Name;
            var dt = ConvertCSVtoDataTable(paramFilePath);
            var newSort = (from row in dt.AsEnumerable()

                           group row by new
                           {
                               Section = row.Field<string>("Section"),
                               Circtuit = row.Field<string>("Circuit"),
                               Ballast = row.Field<string>("Kind Of Ballast/Driver"),
                               length = row.Field<string>("Length"),
                               Workcenter = row.Field<string>("Work Center")
                           } into grp
                           select new
                           {

                               Section = grp.Key.Section,
                               Circtuit = grp.Key.Circtuit,
                               workcenter = grp.Key.Workcenter,
                               Ballast = grp.Key.Ballast,
                               length = grp.Key.length,
                               Sum = grp.Sum(r => Double.Parse(r.Field<string>("Total Time (min)")))
                           }).ToList();
            foreach (var row in newSort)
            {
                Dictionary<string, string> parameters = new Dictionary<string, string>();
                parameters.Add("Section", row.Section);
                parameters.Add("Circuits", row.Circtuit);
                parameters.Add("Driver", row.Ballast);
                int length = Int32.Parse(row.length);
               
                OptionTB.AddInstance(optionName, row.Sum, length);


                foreach (var instace in parameters)
                {
                    ParametersTB.AddInstance(instace.Key, instace.Value);
                    OptionTB.AddParam(optionName, row.Sum, instace.Key, instace.Value,length);
                }
                using (var db = new TimeContext())
                {
                    foreach (var fixture in db.Prod.Where(r => r.WorkCenter == row.workcenter && r.Code == fixturetype))
                    {
                        ProdTB.AddOption(fixturetype, fixture.Code, row.workcenter, optionName, row.Sum, length);

                    }

                }

            }

        }
    }
}