using Axis_ProdTimeDB.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Axis_ProdTimeDB.InputClasses
{
    class Inspection :Utilities
    {
        public Inspection(string paramFilePath)
        {
            string optionName = this.GetType().Name;
            var dt = ConvertCSVtoDataTable(paramFilePath);
            var newSort = (from row in dt.AsEnumerable()

                           group row by new
                           {
                               ID = row.Field<string>("Product ID"),
                               Length = row.Field<string>("Length"),
                               Workcenter = row.Field<string>("Work Center")
                           } into grp
                           //orderby grp.Key
                           select new
                           {
                               Product = grp.Key.ID,
                               length = Int32.Parse(grp.Key.Length),
                               workcenter = grp.Key.Workcenter,

                               Sum = grp.Sum(r => Double.Parse(r.Field<string>("Total time (min)")))
                           }).ToList();


            foreach (var row in newSort)
            {
                ProdTB.AddInstance(prodtype,row.Product, row.workcenter);
                OptionTB.AddInstance(optionName, row.Sum, row.length);
                ProdTB.AddOption(prodtype,row.Product, row.workcenter, optionName, row.Sum, row.length);


            }


        }
    }
}
