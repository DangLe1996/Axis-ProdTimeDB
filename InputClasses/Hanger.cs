using System;
using System.Data;
using System.Linq;

namespace Axis_ProdTimeDB.InputClasses
{
    class Hanger : Utilities
    {

        public Hanger(string paramFilePath)
        {
            string optionName = this.GetType().Name;

            var dt = ConvertCSVtoDataTable(paramFilePath);
            var newSort = (from row in dt.AsEnumerable()

                           group row by new
                           {
                               ID = row.Field<string>("Product ID"),
                               SectionType = row.Field<string>("Section Type"),
                               Workcenter = row.Field<string>("Work Center")
                           } into grp
                           //orderby grp.Key
                           select new
                           {
                               Product = grp.Key.ID,
                               sectionType = grp.Key.SectionType,
                               workcenter = grp.Key.Workcenter,

                               Sum = grp.Sum(r => Double.Parse(r.Field<string>("Total Time (min)")))
                           }).ToList();
            foreach (var row in newSort)
            {
                OptionTB.AddInstance(optionName, row.Sum);
                ProdTB.AddInstance(prodtype, row.Product, row.workcenter);
                ProdTB.AddOption(prodtype, row.Product, row.workcenter, optionName, row.Sum);
                ParametersTB.AddInstance("Section", row.sectionType);
                OptionTB.AddParam(optionName, row.Sum, "Section", row.sectionType);
            }
        }
    }
}
