using Axis_ProdTimeDB.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axis_ProdTimeDB.InputClasses
{
    class SubEndC : Program
    {
        public SubEndC(string paramFilePath)
        {
            string optionName = this.GetType().Name;
            var dt = ConvertCSVtoDataTable(paramFilePath);
            var newSort = (from row in dt.AsEnumerable()

                           group row by new
                           {
                               ID = row.Field<string>("Product ID"),
                               section = row.Field<string>("Section"),
                               Workcenter = row.Field<string>("Work Center")
                           } into grp
                           //orderby grp.Key
                           select new
                           {
                               Product = grp.Key.ID,
                               workcenter = grp.Key.Workcenter,
                               section = grp.Key.section,
                               Sum = grp.Sum(r => Double.Parse(r.Field<string>("Total Time")))
                           }).ToList();



            foreach (var row in newSort)
            {

                ProdTB.AddInstance(row.Product, row.workcenter);
                OptionTB.AddInstance(optionName, row.Sum);
                ParametersTB.AddInstance("SectionType", row.section);
                OptionTB.AddParam(optionName, row.Sum, "SectionType", row.section);
                ProdTB.AddOption(row.Product, row.workcenter, optionName, row.Sum);
            }








        }
    }
}
