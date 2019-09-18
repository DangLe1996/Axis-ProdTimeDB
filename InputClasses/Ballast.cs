using Axis_ProdTimeDB.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axis_ProdTimeDB.InputClasses
{
    class Ballast : Program
    {
        public Ballast(string paramFilePath)
        {
            string optionName = "Ballast";

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
                               Sum = grp.Sum(r => Double.Parse(r.Field<string>("UNIT TIME (MIN)")))
                           }).ToList();


            using (var db = new TimeContext())
            {
                foreach (var row in newSort)
                {

                    InputClass.addProduct(row.Product, row.workcenter);
                    InputClass.addOption(optionName, row.Sum);
                    ProdTB.AddOption(row.Product, row.workcenter, optionName, row.Sum);
                }





            }


        }
    }
}
