using Axis_ProdTimeDB.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axis_ProdTimeDB.InputClasses
{
    class Hanger:Program
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
                ProdTB.AddInstance(row.Product, row.workcenter);
                ParametersTB.AddInstance("SectionType", row.sectionType);
                ProdTB.AddOption(row.Product, row.workcenter, optionName, row.Sum);
                OptionTB.AddParam(optionName, row.Sum, "SectionType", row.sectionType);
            }
        }
    }
}
