using System;
using System.Data;
using System.Linq;

namespace Axis_ProdTimeDB.InputClasses
{
    class SubOp : Utilities
    {
        public SubOp(string paramFilePath)
        {
            string optionName = this.GetType().Name;
            var dt = ConvertCSVtoDataTable(paramFilePath);
            var newSort = (from row in dt.AsEnumerable()

                           group row by new
                           {
                               ID = row.Field<string>("Family Product"),
                               Optic = row.Field<string>("Optic"),
                               Length = row.Field<string>("Length"),
                               section = row.Field<string>("Section"),
                               Workcenter = row.Field<string>("Work Center")
                           } into grp
                           select new
                           {
                               ProdFam = grp.Key.ID,
                               Optic = grp.Key.Optic,
                               Length = grp.Key.Length,
                               workcenter = grp.Key.Workcenter,
                               section = grp.Key.section,
                               Sum = grp.Sum(r => Double.Parse(r.Field<string>("Time (min)")))
                           }).ToList();


            //foreach (var row in newSort)
            //{

            //    ProdFamTB.AddInstance(row.ProdFam, row.workcenter);



            //    OptionTB.AddInstance(optionName, row.Sum, row.Length);

            //    ParametersTB.AddInstance("Optic", row.Optic);
            //    OptionTB.AddParam(optionName, row.Sum, "Optic", row.Optic,row.Length);

            //    ParametersTB.AddInstance("SectionType", row.section);
            //    OptionTB.AddParam(optionName, row.Sum, "SectionType", row.section,row.Length);


            //    ProdFamTB.AddOption(row.ProdFam, row.workcenter, optionName, row.Sum, row.Length);


            //}






        }

    }
}
