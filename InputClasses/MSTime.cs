using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Axis_ProdTimeDB.DAL;
using System.Data;
using System.Transactions;

namespace Axis_ProdTimeDB.InputClasses
{
    class MSTime : Program
    {
        public MSTime(string paramFilePath)
        {
            string optionName = this.GetType().Name;
            var dt = ConvertCSVtoDataTable(paramFilePath);
            var newSort = (from row in dt.AsEnumerable()

                           group row by new
                           {
                               ID = row.Field<string>("Product ID"),
                               Mouting = row.Field<string>("Description"),
                               Length = row.Field<string>("Length"),
                               Workcenter = row.Field<string>("Work Center")
                           } into grp
                           //orderby grp.Key
                           select new
                           {
                               Product = grp.Key.ID,
                               mounting = grp.Key.Mouting,
                               length = grp.Key.Length,
                               workcenter = grp.Key.Workcenter,
                               Sum = grp.Sum(r => Double.Parse(r.Field<string>("Total Time (min)")))
                           }).ToList();


            using (var db = new TimeContext())
            {
                foreach (var row in newSort)
                {
                    var mounting = row.mounting;
                    switch (mounting)
                    {
                        case "Spackle Flange":
                            mounting = "DS";
                            break;
                        case "Flangeless":
                            mounting = "D";
                            break;
                        default:
                            mounting = "-";
                            break;
                    }
                    int length = Int32.Parse(row.length);

                    ProdTB.AddInstance(prodtype,row.Product, row.workcenter);
                    OptionTB.AddInstance(optionName, row.Sum, length);
                    ParametersTB.AddInstance("Mounting", mounting);
                    ProdTB.AddOption(prodtype,row.Product, row.workcenter, optionName, row.Sum, length);
                    OptionTB.AddParam(optionName, row.Sum, "Mounting", mounting);


                }

            }
        }
    }
}
