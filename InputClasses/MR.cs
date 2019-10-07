using Axis_ProdTimeDB.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace Axis_ProdTimeDB.InputClasses
{
    class MR :Utilities
    {
        public MR(string paramFilePath)
        {
            string optionName = this.GetType().Name;
            var dt = ConvertCSVtoDataTable(paramFilePath);
            var newSort = (from row in dt.AsEnumerable()

                           group row by new
                           {
                               ID = row.Field<string>("Fixture Type"),
                          
                               Workcenter = row.Field<string>("Work Center"),
                           
                           } into grp
                           //orderby grp.Key
                           select new
                           {
                               ID = grp.Key.ID,
                            
                               workcenter = grp.Key.Workcenter,

                               Sum = grp.Sum(r => Double.Parse(r.Field<string>("Time(min)")))
                           }).ToList();

            foreach (var row in newSort)
            {
                ProdTB.AddInstance(fixturetype, row.ID, row.workcenter);
                OptionTB.AddInstance(optionName, row.Sum);

                ProdTB.AddOption(fixturetype,row.ID, row.workcenter, optionName, row.Sum);


            }
        }
    }
}
