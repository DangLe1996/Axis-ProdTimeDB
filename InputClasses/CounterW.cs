using Axis_ProdTimeDB.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Axis_ProdTimeDB.InputClasses
{
    class CounterW:Program
    {
        public CounterW(string paramFilePath)
        {
            string optionName = "CounterW";
            var dt = ConvertCSVtoDataTable(paramFilePath);
            var newSort = (from row in dt.AsEnumerable()

                           group row by new
                           {
                               ID = row.Field<string>("Product ID"),
                              
                               Workcenter = row.Field<string>("Work Center")
                           } into grp
                           //orderby grp.Key
                           select new
                           {
                               Product = grp.Key.ID,
                           
                               workcenter = grp.Key.Workcenter,

                               Sum = grp.Sum(r => Double.Parse(r.Field<string>("Time (min)")))
                           }).ToList();
            foreach (var row in newSort)
            {
                ProdTB.AddInstance(row.Product, row.workcenter);
                OptionTB.AddInstance(optionName, row.Sum);
                ProdTB.AddOption(row.Product, row.workcenter, optionName, row.Sum);


            }
        }
    }
}
