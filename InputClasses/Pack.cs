using Axis_ProdTimeDB.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axis_ProdTimeDB.InputClasses
{
    class Pack : Program
    {
        public Pack(string paramFilePath)
        {
            string optionName = this.GetType().Name;
            var dt = ConvertCSVtoDataTable(paramFilePath);

            var newSort = (from row in dt.AsEnumerable()

                           group row by new
                           {
                               ID = row.Field<string>("Product ID"),
                               mounting = row.Field<string>("Description"),
                               Workcenter = row.Field<string>("Work Center"),
                               Length = row.Field<string>("Length")
                           } into grp
                           //orderby grp.Key
                           select new
                           {
                               Product = grp.Key.ID,
                               workcenter = grp.Key.Workcenter,
                               mounting = grp.Key.mounting,
                               Length = grp.Key.Length,
                               Sum = grp.Sum(r => Double.Parse(r.Field<string>("Time (min)")))
                           }).ToList();
            using (var db = new TimeContext())
            {
                foreach (var row in newSort)
                {
                    int length = Int32.Parse(row.Length);
                    string mounting = row.mounting;
                    switch (mounting)
                    {
                        case "Spackle Flange":
                            mounting = "DS";
                            break;
                        case "Flangeless":
                            mounting = "D";
                            break;
                        default:
                            mounting = null;
                            break;
                    }

                    ProdTB.AddInstance(prodtype,row.Product, row.workcenter);
                    
                    OptionTB.AddInstance(optionName, row.Sum, length);
                    if (mounting != null)
                    {
                        ParametersTB.AddInstance("Mounting", mounting);
                        OptionTB.AddParam(optionName, row.Sum, "Mounting", mounting, length);
                    }
                    ProdTB.AddOption(prodtype,row.Product, row.workcenter, optionName, row.Sum);
                   
                }
            }
        }

    }
}
