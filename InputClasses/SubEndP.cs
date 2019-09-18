using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Axis_ProdTimeDB.DAL;
namespace Axis_ProdTimeDB.InputClasses
{
    class SubEndP : Program
    {
        public SubEndP(string paramFilePath)
        {
            string optionName = "SubEndP";
            var dt = ConvertCSVtoDataTable(paramFilePath);
            var newSort = (from row in dt.AsEnumerable()

                           group row by new
                           {
                               ID = row.Field<string>("Product ID"),
                               Mouting = row.Field<string>("Description"),
                               Workcenter = row.Field<string>("Work Center")
                           } into grp
                           //orderby grp.Key
                           select new
                           {
                               Product = grp.Key.ID,
                               mounting = grp.Key.Mouting,
                               workcenter = grp.Key.Workcenter,
                               Sum = grp.Sum(r => Double.Parse(r.Field<string>("Total Time (min)")))
                           }).ToList();
            using (var db = new TimeContext())
            {
                foreach (var row in newSort)
                {
                    var mouting = row.mounting;
                    switch (mouting)
                    {
                        case "Spackle Flange":
                            mouting = "DS";
                            break;
                        case "Flangeless":
                            mouting = "D";
                            break;
                        default:
                            mouting = "-";
                            break;
                    }


                    ProdTB.AddInstance(row.Product, row.workcenter);
                    
                    OptionTB.AddInstance(optionName, row.Sum);
                    ParametersTB.AddInstance("Mounting", mouting);

                    ProdTB.AddOption(row.Product, row.workcenter, optionName, row.Sum);

                    OptionTB.AddParam(optionName, row.Sum, "Mounting", mouting);


                }

            }




        }
    }
}
